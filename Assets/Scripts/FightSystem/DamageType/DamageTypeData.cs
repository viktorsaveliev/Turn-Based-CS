using UnityEngine;

namespace Echobay.FightSystem.DamageType
{
    [CreateAssetMenu(menuName = "Combat/DamageType")]
    public class DamageTypeData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }
}
