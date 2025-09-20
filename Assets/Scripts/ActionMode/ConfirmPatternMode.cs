using Echobay.CardSystem;
using Echobay.GridSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.ActionContext
{
    public class ConfirmPatternMode : TargetSelectionMode
    {
        [SerializeField] private TargetAreaPattern _patternData;
        [SerializeField, Range(0, 5)] private float _completeDelay = 0.5f;

        public override void Enter(ActionContextLinks contextLinks)
        {
            if (_patternData == null)
            {
                Debug.LogError("Pattern data is null. Please ensure the card has a valid pattern.");
                return;
            }

            base.Enter(contextLinks);

            ContextLinks.Grid.SetActiveCells(false);

            ICellOccupant cellOccupant = ContextLinks.ActionController.SelectedUnit;

            CellList = ContextLinks.Grid.GetCellsByPattern(cellOccupant.CurrentCell, _patternData);

            foreach (GridCell cell in CellList)
            {
                cell.SetColor(ContextLinks.ViewData.CellTargetedColor);
            }
        }

        public override string GetDescription()
        {
            return "Click on the card again to confirm";    
        }

        public override void HandleCellClick(GridCell cell)
        {
            
        }

        public override void OnCellEnter(GridCell cell)
        {
            
        }

        public override void OnCellExit(GridCell cell)
        {
            
        }

        public override void OnClickOnCard()
        {
            if (_completeDelay > 0)
            {
                ContextLinks.PathView.StartCoroutine(CompleteDelay());
            }
            else
            {
                Complete();
            }
        }

        private IEnumerator CompleteDelay()
        {
            yield return new WaitForSeconds(_completeDelay);
            Complete();
        }
    }
}
