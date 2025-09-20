using UnityEngine;

namespace Echobay.PlayerSystem
{
    public class CameraShake : MonoBehaviour
    {
        private Camera _camera;

        private Vector3 _originalRotation;
        private float _shakeDuration;
        private float _shakeMagnitude;
        private float _shakeElapsed;
        private bool _isShaking;

        private void Awake()
        {
            _camera = Camera.main;
            _originalRotation = _camera.transform.eulerAngles;
        }

        private void Update()
        {
            if (_isShaking)
            {
                if (_shakeElapsed < _shakeDuration)
                {
                    _shakeElapsed += Time.deltaTime;
                    float shakeAmount = _shakeMagnitude * (1f - (_shakeElapsed / _shakeDuration));

                    Vector3 randomOffset = Random.insideUnitSphere * shakeAmount;
                    _camera.transform.eulerAngles = _originalRotation + new Vector3(randomOffset.x, randomOffset.y, 0);
                }
                else
                {
                    _isShaking = false;
                    _camera.transform.eulerAngles = _originalRotation;
                }
            }
        }

        public void Shake(float duration = 0.5f, float magnitude = 0.3f)
        {
            _shakeDuration = duration;
            _shakeMagnitude = magnitude;
            _shakeElapsed = 0f;
            _isShaking = true;
        }

        public void StopShake()
        {
            _isShaking = false;
            _camera.transform.eulerAngles = _originalRotation;
        }
    }
}
