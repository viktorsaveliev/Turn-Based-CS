using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Echobay.UnitSystem.VisualSystem
{
    public enum Gender
    {
        Unisex = 0,
        Male = 1,
        Female = 2
    }

    public enum VisualSlot
    {
        Hair = 0,
        Brows = 1,
        EyeLeft = 2,
        EyeRight = 3,
        Nose = 4,
        Mouth = 5,
        FaceDetails = 6
    }

    [CreateAssetMenu(menuName = "Units/Visuals/Visual Database")]
    public class VisualDatabase : ScriptableObject
    {
        [ListDrawerSettings]
        public List<VisualElementData> Elements;

        public IEnumerable<VisualElementData> Get(
            VisualSlot slot,
            Gender gender)
        {
            return Elements.Where(e =>
                e.Slot == slot &&
                (e.Gender == Gender.Unisex || e.Gender == gender)
            );
        }

#if UNITY_EDITOR
        [Button]
        private void FindAllObjectData()
        {
            Elements.Clear();
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(VisualElementData)}");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                VisualElementData obj = AssetDatabase.LoadAssetAtPath<VisualElementData>(path);
                Elements.Add(obj);
            }

            Debug.Log($"VisualDatabase updated: found {Elements.Count} items.");
        }
#endif
    }
}
