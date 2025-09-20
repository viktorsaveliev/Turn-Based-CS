using Echobay.GridSystem;
using UnityEngine;

namespace Echobay.ActionContext
{
    public class SelectCellMode : TargetSelectionMode
    {
        public override void HandleCellClick(GridCell cell)
        {
            if (ContextLinks.ActionController.SelectedUnit == null && cell.IsOccupied)
            {
                ContextLinks.ActionController.SelectUnit(cell.Occupant);
                ContextLinks.ActionController.SetContext(new MoveTargetMode());
            }
        }

        public override string GetDescription()
        {
            return "Select cell";
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
        }

        public override void OnCellExit(GridCell cell)
        {
            cell.SetColor(ContextLinks.ViewData.CellRegularColor);
        }

        public override void OnClickOnCard()
        {

        }
    }
}
