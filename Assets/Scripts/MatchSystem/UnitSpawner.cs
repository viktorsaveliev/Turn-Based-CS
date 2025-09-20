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
        [SerializeField] private SpawnData[] _spawnDatas;

        private UnitFactory _factory;

        [Inject]
        public void Construct(UnitFactory factory)
        {
            _factory = factory;
        }

        public void SpawnPlayerUnits(Player player, IReadOnlyList<UnitData> unitDatas)
        {
            SpawnData spawnData = _spawnDatas.FirstOrDefault(s => s.TeamID == player.TeamID);
            if (spawnData == null)
            {
                Debug.LogError($"No spawn data for team {player.TeamID}");
                return;
            }

            int count = Mathf.Min(unitDatas.Count, spawnData.SpawnCells.Length);
            for (int i = 0; i < count; i++)
            {
                Unit unit = _factory.CreateUnit(unitDatas[i], spawnData.SpawnCells[i].transform);
                unit.Init(player.TeamID);
            }
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
            foreach (var cell in GameObject.FindObjectsByType<GridCell>(FindObjectsSortMode.None))
            {
                yield return new ValueDropdownItem<GridCell>($"Cell {cell.Position}", cell);
            }
        }
    }
}
