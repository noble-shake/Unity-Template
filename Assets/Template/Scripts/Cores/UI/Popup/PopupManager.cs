using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

using RottenNoble.Cores.Enum;
using RottenNoble.Cores.Manager;
using RottenNoble.Cores.Resource;

namespace RottenNoble.Cores.UI.Popup
{
    public class PopupManager
    {
        private readonly ResourceFactory resourceFactory;
        private readonly UIManager uiManager;
        private readonly DataManager dataManager;

        private readonly Dictionary<Type, ViewBase> cache = new();
        private readonly Stack<ViewBase> stack = new();

        public PopupManager(ResourceFactory resourceFactory, UIManager uiManager, DataManager dataManager)
        {
            this.resourceFactory = resourceFactory;
            this.uiManager = uiManager;
            this.dataManager = dataManager;
        }

        // ─── Busy ────────────────────────────────────────────────────────────

        public async UniTask ShowBusyAsync(BusyType type = BusyType.Dark, CancellationToken ct = default)
        {
            var path = ResolveBusyPath(type);
            await ShowAsync<PopupBusyView>(ResourceEnum.Local, path, CanvasType.Popup, ct);
        }

        public async UniTask HideBusyAsync(CancellationToken ct = default)
        {
            await HideAsync<PopupBusyView>(ct);
        }

        // ─── Generic ─────────────────────────────────────────────────────────

        public async UniTask<T> ShowAsync<T>(
            ResourceEnum resourceType,
            string path,
            CanvasType canvasType = CanvasType.Popup,
            CancellationToken ct = default) where T : ViewBase
        {
            var popup = await GetOrLoadAsync<T>(resourceType, path, canvasType, ct);
            stack.Push(popup);
            await popup.ShowAsync();
            return popup;
        }

        public async UniTask HideAsync<T>(CancellationToken ct = default) where T : ViewBase
        {
            if (cache.TryGetValue(typeof(T), out var popup) == false) return;

            await popup.HideAsync();

            if (stack.Count > 0 && stack.Peek() == popup)
                stack.Pop();
        }

        public async UniTask HideTopAsync()
        {
            if (stack.Count == 0) return;
            var top = stack.Pop();
            await top.HideAsync();
        }

        public void HideAllImmediate()
        {
            foreach (var popup in cache.Values)
                popup.HideImmediate();
            stack.Clear();
        }

        public bool IsAnyVisible => stack.Count > 0;

        public T Get<T>() where T : ViewBase
        {
            cache.TryGetValue(typeof(T), out var popup);
            return popup as T;
        }

        // ─── Internal ────────────────────────────────────────────────────────

        private string ResolveBusyPath(BusyType type) => type switch
        {
            BusyType.Dark  => dataManager.ResourcePath.Local.Popup.BusyDark,
            BusyType.Light => dataManager.ResourcePath.Local.Popup.BusyLight,
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

        private async UniTask<T> GetOrLoadAsync<T>(
            ResourceEnum resourceType,
            string path,
            CanvasType canvasType,
            CancellationToken ct) where T : ViewBase
        {
            if (cache.TryGetValue(typeof(T), out var cached))
                return cached as T;

            var parent = uiManager.GetCanvasTransform(canvasType);
            var go = await resourceFactory.CreateAsync<GameObject>(resourceType, path, parent);
            var popup = go.GetComponent<T>();
            popup.gameObject.SetActive(false);
            cache[typeof(T)] = popup;
            return popup;
        }
    }
}
