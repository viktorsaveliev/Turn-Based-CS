using Echobay.CardSystem;
using Echobay.GridSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.ActionContext
{
    public class SelectRotationPatternMode : TargetSelectionMode
    {
        [SerializeField] private TargetAreaPattern _patternData;

        public override void Enter(ActionContextLinks contextLinks)
        {
            base.Enter(contextLinks);
            ContextLinks.Grid.SetActiveCells(true);
        }

        public override string GetDescription()
        {
            return "select the angle and confirm";
        }

        public override void HandleCellClick(GridCell cell)
        {
            Complete();
        }

        public override void OnCellEnter(GridCell hoveredCell)
        {
            if (_patternData == null) return;
            if (ContextLinks.ActionController.SelectedUnit == null) return;

            ICellOccupant unit = ContextLinks.ActionController.SelectedUnit;
            GridCell playerCell = unit.CurrentCell;

            Vector2Int dir = hoveredCell.Position - playerCell.Position;
            int rotation = GetRotationFromDirection(dir);

            foreach (var oldCell in CellList)
            {
                if (oldCell == null)
                {
                    continue;
                }

                oldCell.SetColor(ContextLinks.ViewData.CellRegularColor);
            }

            IReadOnlyCollection<Vector2Int> rotatedOffsets = RotateOffsets(_patternData.AffectedCells, rotation);
            CellList = ContextLinks.Grid.GetCellsByOffsets(playerCell, rotatedOffsets);

            foreach (var cell in CellList)
            {
                cell.SetColor(ContextLinks.ViewData.CellTargetedColor);
            }
        }

        public override void OnCellExit(GridCell cell)
        {
            if (!CellList.Contains(cell))
            {
                cell.SetColor(ContextLinks.ViewData.CellRegularColor);
            }
        }

        public override void OnClickOnCard()
        {

        }

        private int GetRotationFromDirection(Vector2Int dir)
        {
            if (dir == Vector2Int.zero) return 0;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                return dir.x > 0 ? 90 : 270;
            }
            else
            {
                return dir.y > 0 ? 0 : 180;
            }
        }

        private List<Vector2Int> RotateOffsets(IEnumerable<Vector2Int> offsets, int rotation)
        {
            List<Vector2Int> result = new();

            foreach (var offset in offsets)
            {
                result.Add(rotation switch
                {
                    0 => offset,
                    90 => new Vector2Int(offset.y, -offset.x),
                    180 => new Vector2Int(-offset.x, -offset.y),
                    270 => new Vector2Int(-offset.y, offset.x),
                    _ => offset
                });
            }

            return result;
        }
    }
}
