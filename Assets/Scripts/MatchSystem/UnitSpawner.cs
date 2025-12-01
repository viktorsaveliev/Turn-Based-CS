using Echobay.GridSystem;
using Echobay.PlayerSystem;
using Echobay.UnitSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Echobay.MatchSystem
{
    public class UnitSpawner : MonoBehaviour
    {
        public IReadOnlyCollection<Unit> Units => _units;

        [SerializeField] private SpawnData[] _spawnDatas;

        private readonly HashSet<Unit> _units = new();

        private UnitFactory _factory;
        private int _unitsCount = 0;

        [Inject]
        public void Construct(UnitFactory factory)
        {
            _factory = factory;
        }

        public void SpawnPlayerUnits(MatchPlayer player, IReadOnlyList<UnitData> unitDatas)
        {
            SpawnData spawnData = _spawnDatas.FirstOrDefault(s => s.TeamID == player.Data.TeamID);
            if (spawnData == null)
            {
                Debug.LogError($"No spawn data for team {player.Data.TeamID}");
                return;
            }

            int count = Mathf.Min(unitDatas.Count, spawnData.SpawnCells.Length);
            for (int i = 0; i < count; i++)
            {
                Unit unit = _factory.CreateUnit(unitDatas[i], spawnData.SpawnCells[i].transform);

                int unitID = _unitsCount;

                unit.Init(player, unitID);
                player.AddUnit(unit);

                spawnData.SpawnCells[i].SetOccupant(unit);

                _units.Add(unit);
                _unitsCount++;
            }
        }

        public bool TryGetUnitByID(int id, out Unit targetUnit)
        {
            targetUnit = null;

            foreach (Unit unit in _units)
            {
                if (unit.UnitID != id) continue;
                targetUnit = unit;
                return true;
            }

            return false;
        }
    }

    [Serializable]
    public class SpawnData
    {
        [Range(-1, 2)] public int TeamID;

        [ValueDropdown(nameof(GetAllCells))]
        public GridCell[] SpawnCells;

        private IEnumerable<ValueDropdownItem<GridCell>> GetAllCells()
        {
            foreach (GridCell cell in GameObject.FindObjectsByType<GridCell>(FindObjectsSortMode.InstanceID))
            {
                yield return new ValueDropdownItem<GridCell>($"Cell {cell.Position}", cell);
            }
        }
    }
}
