using Cysharp.Threading.Tasks;
using Echobay.CardSystem;
using System;
using UnityEngine;
using static Echobay.Contexts;

namespace Echobay.FightSystem.StatusEffects
{
    [Serializable]
    public abstract class StatusEffect
    {
        public event Action<StatusEffect> OnExecuted;

        public StatusEffectData Data { get; private set; }

        public void Init(StatusEffectData data)
        {
            Data = data;
        }

        public virtual void OnApply(ExecuteStatusEffectContext context)
        {
            
        }

        public virtual UniTask OnTurnStart(ExecuteStatusEffectContext context)
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask OnTakeDamage(ExecuteStatusEffectContext context)
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask OnTurnEnd(ExecuteStatusEffectContext context)
        {
            return UniTask.CompletedTask;
        }

        protected async UniTask ExecuteAction(ExecuteActionContext context)
        {
            await Data.Action.Execute(context);
            OnActionExecuted(context);
        }

        protected virtual void OnActionExecuted(ExecuteActionContext context)
        {
            OnExecuted?.Invoke(this);
        }
    }
}