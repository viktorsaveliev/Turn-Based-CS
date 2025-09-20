using Echobay.UnitSystem;
using UnityEngine;
using Zenject;

namespace Echobay
{
    public class UnitFactory
    {
        private readonly DiContainer _container;

        [Inject]
        public UnitFactory(DiContainer container)
        {
            _container = container;
        }

        public Unit CreateUnit(UnitData data, Transform spawnPoint, Transform parent = null)
        {
            Unit unit = _container.InstantiatePrefabForComponent<Unit>(data.Prefab, spawnPoint.position, spawnPoint.rotation, parent);
            return unit;
        }
    }
}
