using UnityEngine;

namespace Echobay.GridSystem
{
    public class PathNode
    {
        public GridCell Cell;

        public PathNode Parent;
        public int GCost;
        public int HCost;
        public int FCost => GCost + HCost;

        public PathNode(GridCell cell)
        {
            Cell = cell;
        }
    }
}
