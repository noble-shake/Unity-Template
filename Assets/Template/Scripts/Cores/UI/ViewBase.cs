using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer;

using RottenNoble.Cores.Enum;

namespace RottenNoble.Cores.UI
{
    public abstract class ViewBase : MonoBehaviour
    {
        [field: SerializeField] public VisibleState VisibleState { get; protected set; } = VisibleState.None;

        protected DisposableBag disposableBag = new DisposableBag();
        protected object[] Parameters { get; private set; }

        private IObjectResolver objectResolver;

        [Inject]
        private void InjectCores(IObjectResolver objectResolver)
        {
            this.objectResolver = objectResolver;
        }

        protected virtual void Awake()
        {
            var rt = transform as RectTransform;
            if (rt != null) rt.anchoredPosition = Vector3.zero;
        }

        protected virtual void OnDestroy()
        {
            disposableBag.Dispose();
        }

        // ─── Show / Hide ──────────────────────────────────────────────────────

        public async UniTask ShowAsync()
        {
            VisibleState = VisibleState.Appearing;
            await OnShowAsync();
            VisibleState = VisibleState.Appeared;
            OnReveal();
        }

        public void ShowImmediate()
        {
            VisibleState = VisibleState.Appeared;
            OnShowImmediate();
            OnReveal();
        }

        public async UniTask HideAsync()
        {
            VisibleState = VisibleState.Disappearing;
            await OnHideAsync();
            VisibleState = VisibleState.Disappeared;
            OnHide();
        }

        public void HideImmediate()
        {
            VisibleState = VisibleState.Disappeared;
            OnHideImmediate();
            OnHide();
        }

        // ─── Override 진입점 ──────────────────────────────────────────────────

        protected virtual UniTask OnShowAsync()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        protected virtual void OnShowImmediate()
        {
            gameObject.SetActive(true);
        }

        protected virtual UniTask OnHideAsync()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }

        protected virtual void OnHideImmediate()
        {
            gameObject.SetActive(false);
        }

        // ─── 콜백 ─────────────────────────────────────────────────────────────

        public virtual void OnReveal() { }
        public virtual void OnHide() { }

        // ─── 초기화 ───────────────────────────────────────────────────────────

        public virtual void Initialize(params object[] parameters)
        {
            Parameters = parameters;
        }

        // ─── DI ───────────────────────────────────────────────────────────────

        public T InjectPresenter<T>() where T : ViewModelCore
        {
            var presenter = gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
            objectResolver.Inject(presenter);
            return presenter;
        }

        public static T Get<T>(string objectName) where T : ViewBase
        {
            var all = FindObjectsByType<ViewBase>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            return all.FirstOrDefault(o => o.name == objectName) as T;
        }
    }
}
