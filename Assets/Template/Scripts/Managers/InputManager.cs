using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RottenNoble.Cores.Input
{
    public class InputManager : MonoBehaviour, InputSchema.ITouchActions
    {
        public ReadOnlyReactiveProperty<Vector2> TouchPosition => _touchPosition;
        private readonly ReactiveProperty<Vector2> _touchPosition = new(Vector2.zero);

        public ReadOnlyReactiveProperty<bool> TouchPressed => _touchPressed;
        private readonly ReactiveProperty<bool> _touchPressed = new(false);

        public ReadOnlyReactiveProperty<Vector2> TouchBeganPosition => _touchBeganPosition;
        private readonly ReactiveProperty<Vector2> _touchBeganPosition = new(Vector2.zero);

        public ReadOnlyReactiveProperty<Vector2> DragDelta => _dragDelta;
        private readonly ReactiveProperty<Vector2> _dragDelta = new(Vector2.zero);

        private InputSchema _inputSchema;
        private bool _inputEnabled = true;

        private void Awake()
        {
            _inputSchema = new InputSchema();
            _inputSchema.Touch.AddCallbacks(this);
        }

        private void OnEnable()
        {
            _inputSchema.Touch.Enable();
        }

        private void OnDisable()
        {
            _inputSchema.Touch.Disable();
        }

        private void OnDestroy()
        {
            _inputSchema.Dispose();
        }

        // ─── ITouchActions ────────────────────────────────────────────────────

        public void OnPress(InputAction.CallbackContext context)
        {
            if (!_inputEnabled) return;

            if (context.started)
            {
                _touchBeganPosition.Value = _touchPosition.Value;
                _touchPressed.Value = true;
            }
            else if (context.canceled)
            {
                _touchPressed.Value = false;
            }
        }

        public void OnPosition(InputAction.CallbackContext context)
        {
            if (!_inputEnabled) return;
            if (context.performed)
                _touchPosition.Value = context.ReadValue<Vector2>();
        }

        public void OnDrag(InputAction.CallbackContext context)
        {
            if (!_inputEnabled) return;
            _dragDelta.Value = context.performed ? context.ReadValue<Vector2>() : Vector2.zero;
        }

        // ─── Control ──────────────────────────────────────────────────────────

        public void SetInputEnabled(bool enabled)
        {
            _inputEnabled = enabled;
            if (!enabled)
            {
                _touchPressed.Value = false;
                _dragDelta.Value = Vector2.zero;
            }
        }
    }
}
