using UnityEngine;
using Zenject;

namespace Echobay.InteractSystem
{
    [RequireComponent(typeof(Outline))]
    public abstract class OutlineObject : InteractableObject
    {
        private ObjectInteractionViewData _visualData;
        private Outline _outline;

        [Inject]
        public void Construct(ObjectInteractionViewData visualDataConfig)
        {
            _visualData = visualDataConfig;
        }

        protected override void Awake()
        {
            base.Awake();

            _outline = GetComponent<Outline>();
            HideOutline();

            ChangeOutlineColorToPointEnter();
        }

        public override void OnPointEnter()
        {
            ShowOutline();
        }

        public override void OnPointExit()
        {
            HideOutline();
        }

        /*public override void OnSelected()
        {
            _outline.OutlineWidth = _visualData.OutlineSizeOnSelect;
            ChangeOutlineColorToSelected();
        }

        public override void OnUnselected()
        {
            HideOutline();

            _outline.OutlineWidth = _visualData.OutlineSizeOnEnter;
            ChangeOutlineColorToPointEnter();
        }*/

        protected void ChangeOutlineColorToPointEnter()
        {
            if (_outline == null || _visualData == null) return;
            _outline.OutlineColor = _visualData.PointEnterOutlineColor;
        }

        protected void ChangeOutlineColorToSelected()
        {
            if (_outline == null || _visualData == null) return;
            _outline.OutlineColor = _visualData.SelectedOutlineColor;
        }

        private void ShowOutline()
        {
            _outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
            _outline.OutlineWidth = _visualData.OutlineSizeOnEnter;
        }

        private void HideOutline()
        {
            _outline.OutlineMode = Outline.Mode.OutlineHidden;
            _outline.OutlineWidth = 0;
        }
    }
}