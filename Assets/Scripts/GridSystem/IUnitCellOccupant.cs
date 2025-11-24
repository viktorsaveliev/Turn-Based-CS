using UnityEngine;

namespace Echobay.GridSystem
{
    public interface IUnitCellOccupant : ICellMoveableOccupant
    {
        public int TeamID { get; }
    }
}
