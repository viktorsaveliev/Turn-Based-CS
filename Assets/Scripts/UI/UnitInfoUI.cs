using Echobay.ActionContext;
using Echobay.UnitSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Echobay.UISystem
{
    public class UnitInfoUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;

        private ActionController _actionController;

        [Inject]
        public void Construct(ActionController actionController)
        {
            _actionController = actionController;
        }

        private void OnEnable()
        {
            _actionController.OnUnitSelected += OnUnitSelected;
        }

        private void OnDisable()
        {
            _actionController.OnUnitSelected -= OnUnitSelected;
        }

        private void OnUnitSelected()
        {
            if (_actionController.SelectedUnit != null)
            {
                Unit unit = (Unit) _actionController.SelectedUnit;
                UnitData data = unit.GetData<UnitData>();

                _name.text = data.Name;
                _description.text = data.Description;
            }
            else
            {
                _name.text = string.Empty;
                _description.text = string.Empty;
            }
        }
    }
}
