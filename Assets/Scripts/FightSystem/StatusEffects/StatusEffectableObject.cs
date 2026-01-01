using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Echobay.Contexts;

namespace Echobay.FightSystem.StatusEffects
{
    public abstract class StatusEffectableObject : DamageableObject
    {
        private readonly List<CharacterEffect> _effects = new();
        private const int DelayBetweenEffects = 1;
        private const int InfinityRemainingTurns = -1;

        private readonly StatusEffectFactory _effectFactory = new();

        private void OnDestroy()
        {
            CleanupEffects();
        }

        public void AddEffect(StatusEffectData statusEffectData)
        {
            int remainingTurns = InfinityRemainingTurns;

            if (statusEffectData.Temporary)
            {
                remainingTurns = statusEffectData.RemainingTurns;
            }

            if (TryGetCharacterEffect(statusEffectData, out CharacterEffect alreadyAddedCharacterEffect))
            {
                alreadyAddedCharacterEffect.RemainingTurns += remainingTurns;
                Debug.Log($"{gameObject.name} increase effect {statusEffectData.Name} for {remainingTurns}. Total: {alreadyAddedCharacterEffect.RemainingTurns}");
            }
            else
            {
                StatusEffect statusEffect = _effectFactory.Create(statusEffectData);

                CharacterEffect characterEffect = new(statusEffect, remainingTurns);

                _effects.Add(characterEffect);

                ExecuteStatusEffectContext context = new()
                {
                    Executer = this
                };

                statusEffect.OnApply(context);
                statusEffect.OnExecuted += OnEffectExecuted;

                Debug.Log($"{gameObject.name} added effect {statusEffectData.Name} in {remainingTurns}");
            }
        }

        public void RemoveEffect(CharacterEffect effect)
        {
            effect.StatusEffect.OnExecuted -= OnEffectExecuted;
            _effects.Remove(effect);
        }

        public async UniTask OnTurnStarted(ExecuteStatusEffectContext context)
        {
            if (_effects.Count <= 0) return;

            for (int i = 0; i < _effects.Count; i++)
            {
                if(_effects[i] == null) continue;

                await _effects[i].StatusEffect.OnTurnStart(context);
                await UniTask.WaitForSeconds(DelayBetweenEffects);
            }
        }

        public async UniTask OnTurnEnded(ExecuteStatusEffectContext context)
        {
            if (_effects.Count <= 0) return;

            for (int i = 0; i < _effects.Count; i++)
            {
                if (_effects[i] == null) continue;

                await _effects[i].StatusEffect.OnTurnEnd(context);
                await UniTask.WaitForSeconds(DelayBetweenEffects);
            }
        }

        public async UniTask OnTakeDamage(ExecuteStatusEffectContext context)
        {
            if (_effects.Count <= 0) return;

            for (int i = 0; i < _effects.Count; i++)
            {
                if (_effects[i] == null) continue;

                await _effects[i].StatusEffect.OnTakeDamage(context);
                await UniTask.WaitForSeconds(DelayBetweenEffects);
            }
        }

        public void CleanupEffects()
        {
            foreach (CharacterEffect effect in _effects)
            {
                effect.StatusEffect.OnExecuted -= OnEffectExecuted;
            }

            _effects.Clear();
        }

        private void OnEffectExecuted(StatusEffect statusEffect)
        {
            if (TryGetCharacterEffect(statusEffect.Data, out CharacterEffect characterEffect))
            {
                if (characterEffect.RemainingTurns == InfinityRemainingTurns) return;

                characterEffect.RemainingTurns--;

                Debug.Log($"{gameObject.name} effect {statusEffect.Data.Name} [{characterEffect.RemainingTurns}]");
                if (characterEffect.RemainingTurns <= 0)
                {
                    RemoveEffect(characterEffect);
                }
            }
        }

        private bool TryGetCharacterEffect(StatusEffectData data, out CharacterEffect characterEffect)
        {
            characterEffect = _effects.FirstOrDefault(e => e.StatusEffect.Data == data);
            return characterEffect != null;
        }
    }

    public class CharacterEffect
    {
        public StatusEffect StatusEffect;
        public int RemainingTurns;

        public CharacterEffect(StatusEffect statusEffect, int remainingTurns)
        {
            StatusEffect = statusEffect;
            RemainingTurns = remainingTurns;
        }
    }
}
