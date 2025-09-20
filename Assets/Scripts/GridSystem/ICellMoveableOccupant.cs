using System;
using System.Collections.Generic;

namespace Echobay.GridSystem
{
    public interface ICellMoveableOccupant : ICellOccupant
    {
        public event Action OnPathCompleted;

        public void MoveAlongPath(List<GridCell> path);
    }
}
