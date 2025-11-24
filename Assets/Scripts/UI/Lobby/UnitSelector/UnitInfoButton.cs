using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Echobay.UnitSystem.Ui
{
    [RequireComponent(typeof(Button))]
    public class UnitInfoButton : MonoBehaviour
    {
        public event Action<UnitInfoButton> OnSelected;

        [SerializeField] private Button _button;
        [SerializeField] private Image _marker;

        [Header("Unit Info")]
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;

        public UnitData Data { get; private set; }

        private void OnValidate()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        public void Init(UnitData unitData, bool isUnitSelected)
        {
            Data = unitData;

            _icon.sprite = Data.Icon;
            _name.text = Data.Name;

            if (isUnitSelected)
            {
                OnUnitSelected();
            }
            else
            {
                OnUnitDeselected();
            }
        }

        public void OnUnitSelected()
        {
            _marker.gameObject.SetActive(true);
        }

        public void OnUnitDeselected()
        {
            _marker.gameObject.SetActive(false);
        }

        private void OnClicked()
        {
            OnSelected?.Invoke(this);
        }
    }
}
