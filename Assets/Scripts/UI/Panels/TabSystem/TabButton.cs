using System;
using UnityEngine;
using UnityEngine.UI;

namespace Echobay.UISystem.TabSystem
{
    [RequireComponent(typeof(Button))]
    public class TabButton : MonoBehaviour
    {
        public event Action<TabButton> OnClicked;

        [SerializeField] private Button _button;
        [SerializeField] private PanelUI _panel;

        private void OnValidate()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClickButton);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClickButton);
        }

        public void SelectTab()
        {
            _panel.Show();
        }

        public void DeselectTab()
        {
            _panel.Hide();
        }

        private void OnClickButton()
        {
            OnClicked?.Invoke(this);
        }
    }
}
