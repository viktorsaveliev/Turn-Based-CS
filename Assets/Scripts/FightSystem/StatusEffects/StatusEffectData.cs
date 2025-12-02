using Echobay.CardSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echobay.FightSystem.StatusEffects
{
    [CreateAssetMenu(fileName = "StatusEffectData", menuName = "Effects/StatusEffectData")]
    public class StatusEffectData : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public bool Temporary { get; private set; } = true;
        [field: SerializeField, Range(1, 10), ShowIf(nameof(Temporary))] public int RemainingTurns { get; private set; } = 1;

        [field: SerializeReference] public StatusEffect EffectPrefab { get; private set; }

        [field: SerializeReference] public CardAction Action { get; private set; }
    }
}