using Echobay.ActionContext;
using TMPro;
using UnityEngine;
using Zenject;

namespace Echobay.UISystem
{
    public class ActionDescriptionText : PanelUI
    {
        [SerializeField] private TMP_Text _description;

        private ActionController _controller;
        private TargetSelectionMode _selectionMode;

        [Inject]
        public void Construct(ActionController controller)
        {
            _controller = controller;
        }

        private void OnEnable()
        {
            _controller.OnSelectionModeChanged += UpdateDescription;
        }

        private void OnDisable()
        {
            _controller.OnSelectionModeChanged -= UpdateDescription;
        }

        private void UpdateDescription(TargetSelectionMode selectionMode)
        {
            _selectionMode = selectionMode;

            if (_selectionMode != null)
            {
                string description = _selectionMode.GetDescription();
                _description.text = description;

                _description.gameObject.SetActive(true);
            }
            else
            {
                _description.gameObject.SetActive(false);
            }
        }
    }
}
