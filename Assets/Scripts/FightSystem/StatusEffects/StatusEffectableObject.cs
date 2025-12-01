using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using static Echobay.Contexts;

namespace Echobay.FightSystem.StatusEffects
{
    public abstract class StatusEffectableObject : MonoBehaviour
    {
        private readonly HashSet<StatusEffect> _effects = new();
        private const float DelayBetweenEffects = 1f;

        private void OnDestroy()
        {
            CleanupEffects();
        }

        public void AddEffect(StatusEffect effect)
        {
            _effects.Add(effect);
            effect.OnApply(this);
            effect.OnExpired += RemoveEffect;
        }

        public void RemoveEffect(StatusEffect effect)
        {
            effect.OnExpired -= RemoveEffect;
            _effects.Remove(effect);
        }

        public async UniTask OnTurnStarted(ExecuteStatusEffectContext context)
        {
            foreach (StatusEffect effect in _effects)
            {
                await effect.OnTurnStart(context);
                await UniTask.WaitForSeconds(DelayBetweenEffects);
            }
        }

        public async UniTask OnTurnEnded(ExecuteStatusEffectContext context)
        {
            foreach (StatusEffect effect in _effects)
            {
                await effect.OnTurnEnd(context);
                await UniTask.WaitForSeconds(DelayBetweenEffects);
            }
        }

        public async UniTask OnTakeDamage(ExecuteStatusEffectContext context)
        {
            foreach (StatusEffect effect in _effects)
            {
                await effect.OnTakeDamage(context);
                await UniTask.WaitForSeconds(DelayBetweenEffects);
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
