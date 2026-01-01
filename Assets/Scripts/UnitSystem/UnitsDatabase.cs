using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Echobay.UnitSystem
{
    [CreateAssetMenu(fileName = "UnitsDatabase", menuName = "Units/Database")]
    public class UnitsDatabase : ScriptableObject
    {
        public IReadOnlyCollection<UnitData> Units => _units;

        [SerializeField] private List<UnitData> _units;

        public bool TryGetUnitDataByID(int id, out UnitData unitData)
        {
            if (_units == null || _units.Count <= id)
            {
                unitData = null;

                Debug.LogError("[UD]: problems detected");
                return false;
            }

            unitData = _units[id];
            return true;
        }

        public bool TryGetUnitID(UnitData unitData, out int unitID)
        {
            unitID = 0;

            if (_units == null)
            {
                Debug.LogError("[UD]: Units database is null");
                return false;
            }

            for (int i = 0; i < _units.Count; i++)
            {
                if (unitData != _units[i]) continue;
                unitID = i;
                return true;
            }

            Debug.LogError($"[UD]: Target unit [{unitData.Name}] was not found");
            return false;
        }

#if UNITY_EDITOR
        [Button]
        private void FindAllObjectData()
        {
            _units.Clear();
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(UnitData)}");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UnitData obj = AssetDatabase.LoadAssetAtPath<UnitData>(path);
                _units.Add(obj);
            }

            Debug.Log($"ObjectDatabase updated: found {_units.Count} items.");
        }
#endif
    }
}
