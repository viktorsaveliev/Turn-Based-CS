using Echobay.MatchSystem.TurnSystem;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Echobay.FightSystem.StatusEffects
{
    public abstract class StatusEffectableObject : MonoBehaviour, ITurnObserver
    {
        private readonly HashSet<StatusEffect> _effects = new();
        private ITurnMaster _master;

        [Inject]
        public void Construct(ITurnMaster turnMaster)
        {
            _master = turnMaster;
        }

        private void OnEnable()
        {
            _master?.Register(this);
        }

        private void OnDisable()
        {
            _master?.Unregister(this);
        }

        private void OnDestroy()
        {
            CleanupEffects();
        }

        public void AddEffect(StatusEffect effect)
        {
            _effects.Add(effect);
            effect.OnApply(this);
            effect.OnExpired += RemoveEffect;

            Debug.Log($"Add effect {effect.GetType()}");
        }

        public void RemoveEffect(StatusEffect effect)
        {
            effect.OnExpired -= RemoveEffect;
            _effects.Remove(effect);
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
            foreach (StatusEffect effect in _effects)
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

        public void CleanupEffects()
        {
            foreach (StatusEffect effect in _effects)
            {
                effect.OnExpired -= RemoveEffect;
            }

            _effects.Clear();
        }
    }
}
