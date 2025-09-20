using Echobay.GridSystem;
using Echobay.CardSystem;
using UnityEngine;
using Echobay.FightSystem;

namespace Echobay.ActionContext
{
    public class SingleTargetSelectionMode : TargetSelectionMode
    {
        public override void Enter(ActionContextLinks contextLinks)
        {
            base.Enter(contextLinks);
            ContextLinks.Grid.SetActiveCells(false);
        }

        public override string GetDescription()
        {
            return "Select target cell";
        }

        public override void OnCellEnter(GridCell cell)
        {
            if (!cell.IsOccupied) return;

            if (TryGetTargetFromCell(cell, out IDamageable _))
            {
                cell.SetColor(ContextLinks.ViewData.CellTargetedColor);
            }
        }

        public override void OnCellExit(GridCell cell)
        {
            if (!cell.IsOccupied) return;

            if (TryGetTargetFromCell(cell, out IDamageable _))
            {
                cell.SetColor(ContextLinks.ViewData.CellRegularColor);
            }
        }

        public override void HandleCellClick(GridCell cell)
        {
            ICardAction action = ContextLinks.ActionController.SelectedAction;
            ICellOccupant character = ContextLinks.ActionController.SelectedUnit;

            ExecuteActionContext context = new(character, cell);
            if (ContextLinks.ActionController.SelectedAction.CanExecute(context))
            {
                action.Execute(context);
                Complete();
            }
        }

        public override void OnClickOnCard()
        {

        }

        private bool TryGetTargetFromCell(GridCell cell, out IDamageable damageable)
        {
            damageable = null;

            if (cell.IsOccupied && cell.Occupant is IDamageable dam)
            {
                damageable = dam;
                return true;
            }

            return false;
        }
    }
}