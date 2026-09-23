using R3;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RottenNoble.Cores.UI
{
    [RequireComponent(typeof(Button))]
    public class StateButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Target")]
        [SerializeField] private Button targetButton;

        [Header("Sprite")]
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite selectedSprite;

        [Header("Background")]
        [SerializeField] private Image bgImage;
        [SerializeField] private Color bgNormal = Color.white;
        [SerializeField] private Color bgDisable = Color.gray;

        [Header("Border")]
        [SerializeField] private Image borderImage;
        [SerializeField] private Color borderNormal = Color.white;
        [SerializeField] private Color borderDisable = Color.gray;

        [Header("Text")]
        [SerializeField] private TMP_Text buttonText;
        [SerializeField] private Color buttonNormal = Color.white;
        [SerializeField] private Color buttonDisable = Color.gray;

        [Header("Selected Color")]
        [SerializeField] private Color bgSelected = Color.white;
        [SerializeField] private Color borderSelected = Color.white;
        [SerializeField] private Color textSelected = Color.white;

        [Header("UnSelected Color")]
        [SerializeField] private Color bgUnSelected = Color.white;
        [SerializeField] private Color borderUnSelected = Color.white;
        [SerializeField] private Color textUnSelected = Color.white;

        [Header("Hover Color")]
        [SerializeField] private Color bgHover = Color.white;
        [SerializeField] private Color borderHover = Color.white;
        [SerializeField] private Color textHover = Color.white;

        private readonly ReactiveProperty<bool> selected = new(false);
        private readonly ReactiveProperty<bool> interactable = new(false);
        private readonly ReactiveProperty<bool> hovered = new(false);

        private Observable<Unit> clickObserver;
        private Sprite fallbackNormalSprite;
        private bool selectionInitialized;

        public Button TargetButton => targetButton;
        public Image BackgroundImage => bgImage;
        public Image BorderImage => borderImage;
        public TMP_Text ButtonText => buttonText;
        public Sprite NormalSprite => normalSprite;
        public Sprite SelectedSprite => selectedSprite;
        public ReadOnlyReactiveProperty<bool> IsSelected => selected;
        public ReadOnlyReactiveProperty<bool> IsInteractable => interactable;
        public ReadOnlyReactiveProperty<bool> IsHovered => hovered;

        #region [ Unity Lifecycles ]

        protected virtual void Reset()
        {
            ResolveTargetReferences();
            CacheNormalSprite();
        }

        protected virtual void Awake()
        {
            ResolveTargetReferences();
            if (targetButton == null)
                return;

            CacheFallbackNormalSprite();
            interactable.Value = targetButton.interactable;
            clickObserver = targetButton.OnClickAsObservable().Share();
            RefreshVisualState();
        }

        protected virtual void Update()
        {
            if (targetButton == null)
                return;

            if (targetButton.interactable == interactable.CurrentValue)
                return;

            interactable.Value = targetButton.interactable;
            RefreshVisualState();
        }

        protected virtual void OnDestroy()
        {
            selected.Dispose();
            interactable.Dispose();
            hovered.Dispose();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            hovered.Value = true;
            RefreshVisualState();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            hovered.Value = false;
            RefreshVisualState();
        }

        #endregion

        private void ResolveTargetReferences()
        {
            targetButton ??= GetComponent<Button>();

            if (targetButton == null || bgImage != null)
                return;

            bgImage = targetButton.targetGraphic as Image;
            bgImage ??= GetComponent<Image>();
        }

        #region [ Public Methods ]

        public virtual Observable<Unit> GetClickObserver()
            => clickObserver;

        public virtual Observable<bool> GetSelectedObserver()
            => selected;

        public virtual Observable<bool> GetInteractableObserver()
            => interactable;

        public virtual Observable<bool> GetHoverObserver()
            => hovered;

        public virtual void SetSelected(bool value)
        {
            selectionInitialized = true;
            selected.Value = value;
            RefreshVisualState();
        }

        public virtual void ResetSelection()
        {
            selectionInitialized = false;
            selected.Value = false;
            RefreshVisualState();
        }

        public virtual void SetInteractable(bool value)
        {
            if (targetButton == null)
                return;

            targetButton.interactable = value;
            interactable.Value = value;
            RefreshVisualState();
        }

        public virtual void SetText(string value)
        {
            if (buttonText == null)
                return;

            buttonText.text = value;
        }

        public virtual void RefreshVisualState()
        {
            ResolveColors(
                out var backgroundColor,
                out var borderColor,
                out var textColor
            );

            if (bgImage != null)
            {
                bgImage.color = backgroundColor;
                RefreshBackgroundSprite();
            }

            if (borderImage != null)
                borderImage.color = borderColor;

            if (buttonText != null)
                buttonText.color = textColor;
        }

        #endregion

        private void CacheNormalSprite()
        {
            if (normalSprite == null && bgImage != null)
                normalSprite = bgImage.sprite;
        }

        private void CacheFallbackNormalSprite()
        {
            if (bgImage != null)
                fallbackNormalSprite = bgImage.sprite;
        }

        private void RefreshBackgroundSprite()
        {
            if (selectedSprite == null)
                return;

            var sprite = selected.CurrentValue
                ? selectedSprite
                : normalSprite != null ? normalSprite : fallbackNormalSprite;

            if (sprite != null)
                bgImage.sprite = sprite;
        }

        private void ResolveColors(
            out Color backgroundColor,
            out Color borderColor,
            out Color textColor
        )
        {
            if (targetButton.interactable == false)
            {
                backgroundColor = bgDisable;
                borderColor = borderDisable;
                textColor = buttonDisable;
                return;
            }

            if (selectionInitialized)
            {
                if (selected.CurrentValue)
                {
                    backgroundColor = bgSelected;
                    borderColor = borderSelected;
                    textColor = textSelected;
                    return;
                }

                if (hovered.CurrentValue)
                {
                    backgroundColor = bgHover;
                    borderColor = borderHover;
                    textColor = textHover;
                    return;
                }

                backgroundColor = bgUnSelected;
                borderColor = borderUnSelected;
                textColor = textUnSelected;
                return;
            }

            if (hovered.CurrentValue)
            {
                backgroundColor = bgHover;
                borderColor = borderHover;
                textColor = textHover;
                return;
            }

            backgroundColor = bgNormal;
            borderColor = borderNormal;
            textColor = buttonNormal;
        }
    }
}
