using System.Collections.Generic;
using UnityEngine;

namespace Echobay.GridSystem
{
    public interface IPathFinder
    {
        public List<GridCell> FindPath(GridCell start, GridCell target);
        public List<GridCell> GetReachableCells(Vector2Int from, int maxCost);
    }
}
