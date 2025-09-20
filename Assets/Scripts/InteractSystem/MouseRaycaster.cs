using Echobay.InputSystem;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace Echobay.PlayerSystem
{
    public class MouseRaycaster : MonoBehaviour, IInteractHandler
    {
        public event Action<IInteractable> OnPointEnter;
        public event Action<IInteractable> OnPointExit;
        public event Action<IInteractable> OnInteract;

        public bool IsPointerOverUI { get; private set; }

        public const float MAX_RAY_DISTANCE = 100;

        [SerializeField] private LayerMask _objectLayerMask;

        private Camera _camera;
        private InputData _inputData;
        private IInteractable _interactable;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            _inputData.OnInteract += Interact;
        }

        private void OnDisable()
        {
            _inputData.OnInteract -= Interact;
        }

        private void FixedUpdate()
        {
            IsPointerOverUI = EventSystem.current.IsPointerOverGameObject();

            if (IsPointerOverUI) return;

            Ray ray = GetRayByMousePosition();

            if (Physics.Raycast(ray, out RaycastHit hitInfo, MAX_RAY_DISTANCE, _objectLayerMask))
            {
                if (hitInfo.collider.TryGetComponent(out IInteractable interactable))
                {
                    if (!interactable.IsInteractable)
                    {
                        ResetInteractable();
                        return;
                    }

                    if (_interactable != null && _interactable != interactable)
                    {
                        _interactable.OnPointExit();

                        OnPointExit?.Invoke(_interactable);
                    }

                    _interactable = interactable;
                    _interactable.OnPointEnter();

                    OnPointEnter?.Invoke(_interactable);
                }
                else
                {
                    ResetInteractable();
                }
            }
            else
            {
                ResetInteractable();
            }
        }

        [Inject]
        public void Construct(InputData inputData)
        {
            _inputData = inputData;
        }

        private Ray GetRayByMousePosition()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            return _camera.ScreenPointToRay(mousePosition);
        }

        private void Interact()
        {
            if (IsPointerOverUI) return;

            if (_interactable != null && _interactable.IsInteractable)
            {
                _interactable.Interact();
                OnInteract?.Invoke(_interactable);
            }
        }

        private void ResetInteractable()
        {
            if (_interactable != null)
            {
                OnPointExit?.Invoke(_interactable);

                _interactable.OnPointExit();
                _interactable = null;
            }
        }
    }
}