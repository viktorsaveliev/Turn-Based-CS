using Sirenix.OdinInspector;
using UnityEngine;

namespace Echobay.UnitSystem.VisualSystem
{
    [CreateAssetMenu(menuName = "Units/Visuals/Visual Element")]
    public class VisualElementData : ScriptableObject
    {
        [Title("Identity")]
        public VisualSlot Slot;

        public Gender Gender;

        [Title("Visual")]
        [PreviewField(Alignment = ObjectFieldAlignment.Left)]
        public GameObject Prefab;

        [Title("Randomization")]
        [MinValue(0)]
        public float Weight = 1f;
    }

}
