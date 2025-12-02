using Echobay.ActionContext;
using Echobay.FightSystem.DamageType;
using Echobay.FightSystem.StatusEffects;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Echobay.CardSystem
{
    public class CardData : ScriptableObject
    {
        [field: Title("Card Data")]
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField, PreviewField(ObjectFieldAlignment.Left, Height = 80)] public Sprite Icon { get; private set; }
        [field: SerializeField, PreviewField(ObjectFieldAlignment.Left, Height = 80)] public Sprite BG { get; private set; }
        [field: SerializeField, Range(0, 5)] public int RequiredActionPoints { get; private set; } = 1;
        [field: SerializeField, Range(0, 5)] public int EnergyCost { get; private set; } = 0;
        [field: SerializeField, Range(0, 10)] public int MaxDistanceByCharacter { get; private set; } = 3;
        [field: SerializeField, Range(0, 5)] public int EnergyGain { get; private set; } = 0;
        [field: SerializeField] public Card Prefab { get; private set; }

        [field: Title("Behaviour")]
        [field: SerializeReference] public DamageTypeData DamageType { get; private set; }
        [field: SerializeReference] public TargetSelectionMode TargetingMode { get; private set; }
        [field: SerializeReference] public CardAction Action { get; private set; }
        [field: Header("Status Effects")]
        [field: SerializeField] public StatusEffectData[] EffectsForTarget { get; private set; } = new StatusEffectData[0];
        [field: SerializeField] public StatusEffectData[] EffectsForAttacker { get; private set; } = new StatusEffectData[0];
    }

    [Serializable]
    public class StatusEffectSettings
    {
        [field: SerializeReference] public TemporaryStatusEffect StatusEffect { get; private set; }
    }
}
