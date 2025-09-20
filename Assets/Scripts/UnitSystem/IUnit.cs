using Echobay.GridSystem;
using System.Collections.Generic;

namespace Echobay.UnitSystem
{
    public interface IUnit
    {
        public void AddSystem(IUnitSystem system);
        public void RemoveSystem(IUnitSystem system);
        public T GetSystem<T>() where T : IUnitSystem;
        public void MoveAlongPath(List<GridCell> path);
    }
}
