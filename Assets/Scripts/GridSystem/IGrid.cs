using Echobay.CardSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.GridSystem
{
    public interface IGrid
    {
        public void ShowGrid();
        public void HideGrid();
        public void ResetGrid();
        public void ShowCells(List<GridCell> cells);
        public void SetActiveCells(bool isActive);
        public void ShowCellsInRadius(GridCell centerCell, int cost);
        public List<GridCell> GetCellsByPattern(GridCell origin, TargetAreaPattern pattern);
        public List<GridCell> GetCellsByOffsets(GridCell origin, IReadOnlyCollection<Vector2Int> offsets);
        public List<GridCell> GetCellsInRadius(GridCell centerCell, int maxSteps);
    }
}
