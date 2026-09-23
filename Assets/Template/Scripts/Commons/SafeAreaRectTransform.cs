using UnityEngine;

namespace RottenNoble.Cores.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaRectTransform : MonoBehaviour
    {
        [Header("Padding (Canvas Units)")]
        public float horizontalPadding = 0f;

#if UNITY_EDITOR
        [Header("Simulate Safe Area (Editor Only)")]
        public bool useSimulatedSafeArea = false;
        public Rect simulatedSafeArea = new Rect(0, 0, 1080, 1920);
#endif

        private RectTransform rectTransform;
        private Rect lastSafeArea = Rect.zero;
        private Vector2Int lastScreenSize = Vector2Int.zero;
        private ScreenOrientation lastOrientation = ScreenOrientation.Unknown;
        private float lastHorizontalPadding = float.MinValue;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        void Update()
        {
            if (RefreshNeeded()) ApplySafeArea();
        }

        private bool RefreshNeeded()
        {
            if (lastSafeArea != Screen.safeArea) return true;
            if (lastScreenSize.x != Screen.width || lastScreenSize.y != Screen.height) return true;
            if (lastOrientation != Screen.orientation) return true;
            if (lastHorizontalPadding != horizontalPadding) return true;
            return false;
        }

        public void ApplySafeArea()
        {
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

#if UNITY_EDITOR
            Rect safeArea = useSimulatedSafeArea ? simulatedSafeArea : Screen.safeArea;
#else
            Rect safeArea = Screen.safeArea;
#endif
            float screenW = Screen.width;
            float screenH = Screen.height;
            if (screenW == 0 || screenH == 0) return;

            lastSafeArea = Screen.safeArea;
            lastScreenSize.x = Screen.width;
            lastScreenSize.y = Screen.height;
            lastOrientation = Screen.orientation;
            lastHorizontalPadding = horizontalPadding;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= screenW; anchorMin.y /= screenH;
            anchorMax.x /= screenW; anchorMax.y /= screenH;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = new Vector2(horizontalPadding, rectTransform.offsetMin.y);
            rectTransform.offsetMax = new Vector2(-horizontalPadding, rectTransform.offsetMax.y);
        }
    }
}
