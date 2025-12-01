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
        public event Action<StatusEffect> OnExpired;
        public event Action<StatusEffect> OnExecuted;

        [field: SerializeReference] protected CardAction Action { get; private set; }

        protected StatusEffectableObject StatusObject { get; private set; }

        public virtual void OnApply(StatusEffectableObject statusObject)
        { 
            StatusObject = statusObject;
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

        protected virtual void OnExpire()
        {
            OnExpired?.Invoke(this);
        }

        protected async UniTask ExecuteAction(ExecuteActionContext context)
        {
            await Action.Execute(context);
            OnActionExecuted(context);
        }

        protected virtual void OnActionExecuted(ExecuteActionContext context)
        {
            OnExecuted?.Invoke(this);
        }
    }
}