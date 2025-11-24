using Echobay.CardSystem;
using Echobay.UISystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Echobay.UnitSystem.Ui
{
    public class UnitSelectorUI : PanelUI
    {
        [Header("Units List")]
        [SerializeField] private UnitInfoButton _unitInfoPrefab;
        [SerializeField] private Transform _unitsContainer;
        [SerializeField] private TMP_Text _selectedUnitsText;

        [Header("Unit Info")]
        [SerializeField] private Image _unitIcon;
        [SerializeField] private TMP_Text _unitNameText;
        [SerializeField] private TMP_Text _unitDescText;
        [SerializeField] private Transform _cardsContainer;

        [Header("Others")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _equipButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private TMP_Text _equipButtonText;

        private UnitsDatabase _unitsDatabase;
        private SelectUnitHandler _selectUnitHandler;

        private UnitInfoButton _selectedUnit;

        [Inject]
        public void Construct(UnitsDatabase unitsDatabase, SelectUnitHandler selectUnitHandler)
        {
            _unitsDatabase = unitsDatabase;
            _selectUnitHandler = selectUnitHandler;
        }

        public override void Show()
        {
            base.Show();

            _selectUnitHandler.SyncWithLocalPlayer();

            CreateList();

            _closeButton.onClick.AddListener(OnClickCloseButton);
            _equipButton.onClick.AddListener(OnClickEquipButton);
            _resetButton.onClick.AddListener(OnClickResetButton);
        }

        public override void Hide()
        {
            base.Hide();

            _selectedUnit = null;

            DestroyList();
            DestroyCards();

            _closeButton.onClick.RemoveListener(OnClickCloseButton);
            _equipButton.onClick.RemoveListener(OnClickEquipButton);
            _resetButton.onClick.RemoveListener(OnClickResetButton);
        }

        private void OnClickEquipButton()
        {
            if (_selectedUnit == null) return;

            if (_unitsDatabase.TryGetUnitID(_selectedUnit.Data, out int unitID))
            {
                if (_selectUnitHandler.HasUnit(unitID))
                {
                    _selectUnitHandler.TryRemoveUnit(unitID);
                    _selectedUnit.OnUnitDeselected();
                }
                else
                {
                    if (_selectUnitHandler.TryAddUnit(unitID))
                    {
                        _selectedUnit.OnUnitSelected();
                    }
                    else
                    {
                        Debug.Log("You dont have free slot");
                    }
                }

                int unitsCount = _selectUnitHandler.GetOccupiedSlotCount();
                Color textColor = unitsCount >= SelectUnitHandler.MaxUnits ? Color.green : Color.red;

                _selectedUnitsText.text = $"Selected Units: {unitsCount}/{SelectUnitHandler.MaxUnits}";
                _selectedUnitsText.color = textColor;

                UpdateEquipButton();
            }
        }

        private void OnClickCloseButton()
        {
            if (!_selectUnitHandler.IsFullySlots())
            {
                Debug.Log($"You need select {SelectUnitHandler.MaxUnits} units");
                return;
            }

            _selectUnitHandler.ConfirmSelection();
            Hide();
        }

        private void OnClickResetButton()
        {
            _selectUnitHandler.CancelSelection();
            RefreshUnitButtons();
        }

        private void RefreshUnitButtons()
        {
            UnitInfoButton[] buttons = _unitsContainer.GetComponentsInChildren<UnitInfoButton>();

            foreach (UnitInfoButton button in buttons)
            {
                RefreshUnitButton(button);
            }

            int unitsCount = _selectUnitHandler.GetOccupiedSlotCount();
            Color textColor = unitsCount >= SelectUnitHandler.MaxUnits ? Color.green : Color.red;

            _selectedUnitsText.text = $"Selected Units: {unitsCount}/{SelectUnitHandler.MaxUnits}";
            _selectedUnitsText.color = textColor;

            UpdateEquipButton();
        }

        private void RefreshUnitButton(UnitInfoButton button)
        {
            bool isSelected = _selectUnitHandler.HasUnit(button.Data);

            if (isSelected)
            {
                button.OnUnitSelected();
            }
            else
            {
                button.OnUnitDeselected();
            }
        }

        private void CreateList()
        {
            DestroyList();

            foreach (UnitData unitData in _unitsDatabase.Units)
            {
                UnitInfoButton unitInfoButton = Instantiate(_unitInfoPrefab, _unitsContainer);
                unitInfoButton.Init(unitData, _selectUnitHandler.HasUnit(unitData));

                RefreshUnitButton(unitInfoButton);
                unitInfoButton.OnSelected += ShowUnitInfo;
            }
        }

        private void DestroyList()
        {
            UnitInfoButton[] unitInfoButtons = _unitsContainer.GetComponentsInChildren<UnitInfoButton>();

            foreach (UnitInfoButton button in unitInfoButtons)
            {
                button.OnSelected -= ShowUnitInfo;

                Destroy(button.gameObject);
            }
        }

        private void UpdateEquipButton()
        {
            if (_selectedUnit == null) return;

            if (_unitsDatabase.TryGetUnitID(_selectedUnit.Data, out int unitID))
            {
                if (_selectUnitHandler.HasUnit(unitID))
                {
                    _equipButtonText.text = "Unequip";
                }
                else
                {
                    _equipButtonText.text = "Equip";
                }
            }
        }

        private void ShowUnitInfo(UnitInfoButton unitInfoButton)
        {
            UnitData data = unitInfoButton.Data;

            _unitIcon.sprite = data.Icon;
            _unitNameText.text = data.Name;
            _unitDescText.text = data.Description;

            DestroyCards();

            foreach (CardData cardData in data.CardsList)
            {
                Card card = Instantiate(cardData.Prefab, _cardsContainer);
                card.Init(cardData);
            }

            _selectedUnit = unitInfoButton;

            UpdateEquipButton();
        }

        private void DestroyCards()
        {
            Card[] cards = _cardsContainer.GetComponentsInChildren<Card>();

            foreach (Card card in cards)
            {
                Destroy(card.gameObject);
            }
        }
    }
}
