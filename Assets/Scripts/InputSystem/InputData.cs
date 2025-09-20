using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Echobay.InputSystem
{
    public class InputData : IDisposable
    {
        public event Action<bool> OnScrollWheel;

        public event Action OnInteract;
        public event Action OnRotated;
        public event Action OnCanceled;

        private PlayerInputActions _input;

        public Vector2 Move { get; private set; }
        public Vector2 Scroll { get; private set; }

        public void Init()
        {
            _input = new();
            _input.Enable();

            _input.Player.Move.performed += OnMove;
            _input.Player.Move.canceled += OnMove;

            _input.Player.Interact.canceled += Interact;
            _input.Player.ScrollWheel.performed += OnScroll;
            _input.Player.ScrollWheel.canceled += OnScroll;

            _input.Player.Rotate.performed += OnRotate;
            _input.Player.Cancel.performed += OnCancel;
        }

        public void Dispose()
        {
            _input.Player.Move.performed -= OnMove;
            _input.Player.Move.canceled -= OnMove;

            _input.Player.Interact.canceled -= Interact;
            _input.Player.ScrollWheel.performed -= OnScroll;
            _input.Player.ScrollWheel.canceled -= OnScroll;

            _input.Player.Rotate.performed -= OnRotate;
            _input.Player.Cancel.performed -= OnCancel;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 movementInput = context.ReadValue<Vector2>();
                Move = movementInput;
            }
            else
            {
                Move = Vector2.zero;
            }
        }

        private void OnScroll(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 scroll = context.ReadValue<Vector2>();
                Scroll = scroll;
            }
            else
            {
                Scroll = Vector2.zero;
            }

            OnScrollWheel?.Invoke(context.performed);
        }

        private void Interact(InputAction.CallbackContext context)
        {
            OnInteract?.Invoke();
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            OnCanceled?.Invoke();
        }

        private void OnRotate(InputAction.CallbackContext context)
        {
            OnRotated?.Invoke();
        }
    }
}