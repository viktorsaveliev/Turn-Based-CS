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
                if (cell.Occupant is IUnitCellOccupant unit)
                {
                    if (ContextLinks.Player.Data.TeamID != unit.TeamID)
                    {
                        Debug.Log("It is enemy unit");
                        return;
                    }

                    ContextLinks.ActionController.SelectUnit(unit);
                    ContextLinks.ActionController.SetSelectionMode(new MoveTargetMode());
                }
                else
                {
                    Debug.LogError($"occupant not unit {cell.Occupant}");
                }
            }
        }

        public override string GetDescription()
        {
            return "Select unit";
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
