using Echobay.GridSystem;
using UnityEngine;

namespace Echobay.ActionContext
{
    public class MoveTargetMode : TargetSelectionMode
    {
        public override void Enter(ActionContextLinks contextLinks)
        {
            base.Enter(contextLinks);

            ContextLinks.Grid.SetActiveCells(false);
            ContextLinks.Grid.ShowCellsInRadius(ContextLinks.ActionController.SelectedUnit.CurrentCell, 3);
        }

        public override void HandleCellClick(GridCell cell)
        {
            if (ContextLinks.ActionController.SelectedUnit == cell.Occupant)
            {
                ContextLinks.ActionController.CancelAction();
                return;
            }

            ContextLinks.Grid.SetActiveCells(false);
            ContextLinks.ActionController.RequestMove(cell);
        }

        public override string GetDescription()
        {
            return "Select an available cell to move to";
        }

        public override void OnCellEnter(GridCell cell)
        {
            if (cell.IsOccupied)
            {
                cell.SetColor(ContextLinks.ViewData.CellOccupiedColor);
            }
            else
            {
                cell.SetColor(ContextLinks.ViewData.CellSelectedColor);
            }

            ContextLinks.PathView.ShowPath(cell);
        }

        public override void OnCellExit(GridCell cell)
        {
            cell.SetColor(ContextLinks.ViewData.CellRegularColor);
            ContextLinks.PathView.ClearPath();
        }

        public override void OnClickOnCard()
        {
            ContextLinks.PathView.ClearPath();
        }
    }
}
