using System.Collections.Generic;
using UnityEngine;

namespace Echobay.FightSystem.StatusEffects
{
    public abstract class StatusEffectableObject : MonoBehaviour
    {
        private readonly HashSet<StatusEffect> _effects = new();

        public void AddEffect(StatusEffect effect)
        {
            _effects.Add(effect);
            effect.OnApply(this);
        }

        public void RemoveEffect(StatusEffect statusEffect)
        {
            _effects.Remove(statusEffect);
        }

        public void OnTurnStarted()
        {
            foreach (StatusEffect effect in _effects)
            {
                effect.OnTurnStart();
            }
        }

        public void OnTurnEnded()
        {
            foreach(StatusEffect effect in _effects)
            {
                effect.OnTurnEnd();
            }
        }

        public void OnTakeDamage(StatusEffectableObject attacker)
        {
            foreach (StatusEffect effect in _effects)
            {
                effect.OnTakeDamage(attacker);
            }
        }
    }
}
