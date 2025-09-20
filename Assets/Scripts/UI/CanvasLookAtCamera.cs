using UnityEngine;

namespace Echobay.UISystem
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasLookAtCamera : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        private Transform _target;

        private void OnValidate()
        {
            if (_canvas == null)
            {
                _canvas = GetComponent<Canvas>();
            }
        }

        private void Awake()
        {
            Camera camera = Camera.main;
            _canvas.worldCamera = camera;
            _target = camera.transform;
        }

        private void Update()
        {
            transform.LookAt(_target);
        }
    }
}
