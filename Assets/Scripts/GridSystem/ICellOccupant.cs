using UnityEngine;

namespace Echobay.GridSystem
{
    public interface ICellOccupant
    {
        public GridCell CurrentCell { get; set; }
    }
}
