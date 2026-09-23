using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

using RottenNoble.Cores.Enum;
using RottenNoble.Cores.Resource;

namespace RottenNoble.Cores.UI.Navigation
{
    public class UINavigationManager
    {
        private readonly ResourceFactory resourceFactory;
        private readonly UIManager uiManager;

        private readonly Stack<CachedEntry> navigationStack = new();
        private readonly LinkedList<CachedEntry> hiddenCache = new();

        public bool CanPop => navigationStack.Count > 1;

        public UINavigationManager(ResourceFactory resourceFactory, UIManager uiManager)
        {
            this.resourceFactory = resourceFactory;
            this.uiManager = uiManager;
        }

        // ─── Push ─────────────────────────────────────────────────────────────

        public async UniTask<TViewModel> PushAsync<TView, TViewModel, TModel>(
            ResourceEnum resourceType,
            string path,
            TModel model,
            CanvasType canvasType = CanvasType.Hud,
            Func<UniTask> onComplete = null,
            Action onReveal = null,
            Action onHide = null,
            CancellationToken ct = default,
            IObjectResolver injectionResolver = null)
            where TView      : ViewBase
            where TViewModel : ViewModelBase<TView, TModel>
            where TModel     : ModelBase
        {
            // 현재 뷰를 HiddenCache 맨 뒤에 보관
            // 스택 상단이 null이면 씬 전환으로 파괴된 것 → 스택 전체 초기화
            if (navigationStack.Count > 0)
            {
                if (navigationStack.Peek().View == null)
                {
                    navigationStack.Clear();
                    hiddenCache.Clear();
                }
                else
                {
                    var current = navigationStack.Peek();
                    current.View.HideImmediate();
                    hiddenCache.AddLast(current);
                }
            }

            // HiddenCache에서 같은 타입 찾으면 복원, 없으면 새로 로드
            var cachedNode = FindInHiddenCache(typeof(TView));
            CachedEntry entry;
            TViewModel viewModel;

            if (cachedNode != null)
            {
                entry = cachedNode.Value;
                hiddenCache.Remove(cachedNode);
                viewModel = entry.View.GetComponent<TViewModel>();
                viewModel?.UpdateModel(model);
                entry.OnReveal = onReveal;
                entry.OnHide   = onHide;
            }
            else
            {
                var parent = uiManager.GetSafeAreaTransformInCanvas(canvasType);
                var go = await resourceFactory.CreateAsync<GameObject>(
                    resourceType,
                    path,
                    parent,
                    injectionResolver);

                var view = go.GetComponent<TView>();
                // 모든 자식 컴포넌트의 Awake가 완료된 뒤 ViewModel 초기화를 시작한다.
                // ViewBase.Awake에서 루트를 먼저 비활성화하면 자식 StateButton의 Awake가
                // 건너뛰어질 수 있으므로, 생성 직후 여기서 초기 상태를 숨긴다.
                view.gameObject.SetActive(false);
                view.Initialize();

                viewModel = view.InjectPresenter<TViewModel>();
                viewModel.OnComplete = onComplete;
                await viewModel.Initialize(view, model);

                entry = new CachedEntry
                {
                    View     = view,
                    OnReveal = onReveal,
                    OnHide   = onHide,
                };
            }

            navigationStack.Push(entry);
            await entry.View.ShowAsync();
            entry.OnReveal?.Invoke();

            return viewModel;
        }

        public UniTask<TViewModel> PushAsync<TView, TViewModel, TModel>(
            ResourceAddress address,
            TModel model,
            CanvasType canvasType = CanvasType.Hud,
            Func<UniTask> onComplete = null,
            Action onReveal = null,
            Action onHide = null,
            CancellationToken ct = default,
            IObjectResolver injectionResolver = null)
            where TView      : ViewBase
            where TViewModel : ViewModelBase<TView, TModel>
            where TModel     : ModelBase
            => PushAsync<TView, TViewModel, TModel>(
                address.Type,
                address.Path,
                model,
                canvasType,
                onComplete,
                onReveal,
                onHide,
                ct,
                injectionResolver);

        // ─── Pop ──────────────────────────────────────────────────────────────

        public async UniTask PopAsync(CancellationToken ct = default)
        {
            if (navigationStack.Count == 0) return;

            var current = navigationStack.Pop();
            await current.View.HideAsync();
            current.OnHide?.Invoke();

            RemoveFromHiddenCache(current.View.GetType());

            if (navigationStack.Count > 0)
            {
                var previous = navigationStack.Peek();
                RemoveFromHiddenCache(previous.View.GetType());
                await previous.View.ShowAsync();
                previous.OnReveal?.Invoke();
            }
        }

        public async UniTask PopToRootAsync(CancellationToken ct = default)
        {
            while (navigationStack.Count > 1)
            {
                var entry = navigationStack.Pop();
                entry.View.HideImmediate();
                RemoveFromHiddenCache(entry.View.GetType());
            }

            if (navigationStack.Count > 0)
            {
                var root = navigationStack.Peek();
                RemoveFromHiddenCache(root.View.GetType());
                await root.View.ShowAsync();
                root.OnReveal?.Invoke();
            }
        }

        // ─── Anchor To Root ───────────────────────────────────────────────────

        public async UniTask PopAnchorToRootAsync<TView>(CancellationToken ct = default) where TView : ViewBase
        {
            var targetType = typeof(TView);

            // 스택에서 대상 뷰 위에 있는 것들 모두 닫기
            while (navigationStack.Count > 0 && navigationStack.Peek().View.GetType() != targetType)
            {
                var entry = navigationStack.Pop();
                entry.View.HideImmediate();
                entry.OnHide?.Invoke();
                RemoveFromHiddenCache(entry.View.GetType());
            }

            // 스택에 대상 뷰가 있으면 표시
            if (navigationStack.Count > 0 && navigationStack.Peek().View.GetType() == targetType)
            {
                var target = navigationStack.Peek();
                await target.View.ShowAsync();
                target.OnReveal?.Invoke();
                return;
            }

            // 스택에 없으면 HiddenCache에서 탐색
            var node = FindInHiddenCache(targetType);
            if (node == null) return;

            // Cache에서 대상 뷰 이후에 추가된 것들 제거
            var current = hiddenCache.Last;
            while (current != null && current != node)
            {
                var prev = current.Previous;
                current.Value.View.HideImmediate();
                current.Value.OnHide?.Invoke();
                hiddenCache.Remove(current);
                current = prev;
            }

            var restored = node.Value;
            hiddenCache.Remove(node);

            navigationStack.Push(restored);
            await restored.View.ShowAsync();
            restored.OnReveal?.Invoke();
        }

        // ─── Clear ────────────────────────────────────────────────────────────

        public void ClearAll(ResourceEnum resourceType)
        {
            foreach (var entry in navigationStack)
                resourceFactory.DeleteInstance(resourceType, entry.View.gameObject);
            navigationStack.Clear();

            foreach (var entry in hiddenCache)
                resourceFactory.DeleteInstance(resourceType, entry.View.gameObject);
            hiddenCache.Clear();
        }

        // ─── Query ────────────────────────────────────────────────────────────

        public TView Get<TView>() where TView : ViewBase
        {
            foreach (var entry in navigationStack)
                if (entry.View is TView view) return view;

            var node = FindInHiddenCache(typeof(TView));
            return node?.Value.View as TView;
        }

        // ─── Internal ─────────────────────────────────────────────────────────

        private LinkedListNode<CachedEntry> FindInHiddenCache(Type type)
        {
            var node = hiddenCache.First;
            while (node != null)
            {
                if (node.Value.View.GetType() == type) return node;
                node = node.Next;
            }
            return null;
        }

        private void RemoveFromHiddenCache(Type type)
        {
            var node = FindInHiddenCache(type);
            if (node != null) hiddenCache.Remove(node);
        }

        // ─── CachedEntry ──────────────────────────────────────────────────────

        private class CachedEntry
        {
            public ViewBase View;
            public Action OnReveal;
            public Action OnHide;
        }
    }
}
