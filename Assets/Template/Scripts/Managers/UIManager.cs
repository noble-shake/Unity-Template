using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using RottenNoble.Cores.Enum;

namespace RottenNoble.Cores.UI
{
    public class UIManager : MonoBehaviour
    {
        private readonly string Tag = $"[{nameof(UIManager)}]";

        [SerializeField] private List<CanvasSet> canvases;

        public Canvas GetCanvas(CanvasType type)
            => canvases.FirstOrDefault(o => o.CanvasType == type)?.Canvas;

        public void ShowBackground() => GetCanvas(CanvasType.Background)?.gameObject.SetActive(true);
        public void HideBackground() => GetCanvas(CanvasType.Background)?.gameObject.SetActive(false);

        public Transform GetCanvasTransform(CanvasType type)
            => GetCanvas(type)?.transform;

        public Transform GetSafeAreaTransformInCanvas(CanvasType type)
            => canvases.Find(o => o.CanvasType == type)?.SafeAreaContainer;

        public void Set(CanvasType type, GameObject uiRoot)
        {
            var safeArea = GetSafeAreaTransformInCanvas(type);
            if (safeArea) uiRoot.transform.SetParent(safeArea, false);
        }

        public void SetCanvasAsParent(CanvasType type, Transform source)
        {
            var canvas = GetCanvas(type);
            if (canvas) source.SetParent(canvas.transform, true);
        }

        public T GetView<T>(CanvasType type) where T : ViewBase
        {
            var canvas = GetCanvas(type);
            return canvas?.GetComponentsInChildren<T>(true).FirstOrDefault();
        }

        public void ClearCanvas(CanvasType type)
        {
            var safeArea = GetSafeAreaTransformInCanvas(type);
            if (safeArea == null) return;
            foreach (Transform child in safeArea) Destroy(child.gameObject);
        }

        public void ClearAllCanvas()
        {
            foreach (var set in canvases)
            {
                var safeArea = GetSafeAreaTransformInCanvas(set.CanvasType);
                if (safeArea == null) continue;
                foreach (Transform child in safeArea) Destroy(child.gameObject);
            }
        }
    }

    [Serializable]
    public class CanvasSet
    {
        [field: SerializeField] public CanvasType CanvasType { get; private set; }
        [field: SerializeField] public Canvas Canvas { get; private set; }
        [field: SerializeField] public Transform SafeAreaContainer { get; private set; }

        public static implicit operator bool(CanvasSet c) => c != null;
    }
}
