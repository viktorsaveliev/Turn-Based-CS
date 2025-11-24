using System;
using UnityEngine;
using UnityEngine.UI;

namespace Echobay.Lobby
{
    [RequireComponent(typeof(Button))]
    public class ReadyButton : MonoBehaviour
    {
        public event Action<bool> OnReadyStateChanged;

        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private Color _readyColor;
        [SerializeField] private Color _notReadyColor;

        private bool _ready;

        private void OnValidate()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(ToggleState);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ToggleState);
            SetState(false);
        }

        public void SetState(bool ready)
        {
            _ready = ready;

            if (_ready)
            {
                _image.color = _readyColor;
            }
            else
            {
                _image.color = _notReadyColor;
            }
        }

        private void ToggleState()
        {
            _ready = !_ready;
            OnReadyStateChanged?.Invoke(_ready);
        }
    }
}
