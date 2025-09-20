using Echobay.InputSystem;
using UnityEngine;
using Zenject;

namespace Echobay.PlayerSystem
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private MouseRaycaster _mouseRaycaster;

        [Header("Speeds")]
        [SerializeField, Range(0.5f, 20f)] private float _moveSpeed = 5f;
        [SerializeField, Range(0.2f, 3f)] private float _zoomSpeed = 1f;

        [Header("Limits")]
        [SerializeField] private Vector2 _minLimitMove;
        [SerializeField] private Vector2 _maxLimitMove;

        private IInteractHandler _objectFinder;
        private InputData _inputData;

        private const float _minScrollY = 5f;
        private const float _maxScrollY = 15f;

        private Vector3 _velocity = Vector3.zero;

        private Vector3 _targetPosition;
        private Camera _camera;

        [Inject]
        public void Construct(InputData inputData)
        {
            _inputData = inputData;
        }

        private void Awake()
        {
            _camera = Camera.main;
            _targetPosition = _camera.transform.position;

            _objectFinder = _mouseRaycaster.GetComponent<IInteractHandler>();
        }

        private void OnEnable()
        {
            _inputData.OnScrollWheel += OnScroll;
        }

        private void OnDisable()
        {
            _inputData.OnScrollWheel -= OnScroll;
        }

        private void Update()
        {
            MoveCamera();
        }

        private void OnScroll(bool performed)
        {
            if (_objectFinder.IsPointerOverUI) return;

            float zoomAmount = _inputData.Scroll.y * _zoomSpeed * Time.deltaTime;
            Vector3 verticalMovement = Vector3.up * zoomAmount;
            _targetPosition -= verticalMovement;
            _targetPosition.y = Mathf.Clamp(_targetPosition.y, _minScrollY, _maxScrollY);
        }

        private void MoveCamera()
        {
            if (_objectFinder.IsPointerOverUI) return;

            Vector3 movement = _moveSpeed * Time.deltaTime * new Vector3(_inputData.Move.y, 0, -_inputData.Move.x);
            Vector3 newPosition = _camera.transform.position + movement;

            newPosition.x = Mathf.Clamp(newPosition.x, _minLimitMove.x, _maxLimitMove.x);
            newPosition.z = Mathf.Clamp(newPosition.z, _minLimitMove.y, _maxLimitMove.y);

            _camera.transform.position = newPosition;
        }

    }

}