using Echobay.UnitSystem;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Echobay.Lobby
{
    public class UnitList : MonoBehaviour
    {
        [SerializeField] private WarriorData[] _warriors;

#if UNITY_EDITOR
        [Button]
        private void FindAllWarriors()
        {
            string[] guids = AssetDatabase.FindAssets("t:WarriorData");
            _warriors = new WarriorData[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                _warriors[i] = AssetDatabase.LoadAssetAtPath<WarriorData>(path);
            }
            Debug.Log($"Found {_warriors.Length} WarriorData assets.");
        }
#endif
    }
}
