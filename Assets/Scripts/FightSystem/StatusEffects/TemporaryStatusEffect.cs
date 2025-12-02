using Echobay.CardSystem;
using UnityEngine;

namespace Echobay.FightSystem.StatusEffects
{
    public abstract class TemporaryStatusEffect : StatusEffect
    {
        [field: SerializeField, Range(1, 5)] public int RemainingTurns { get; private set; } = 1;
    }
}