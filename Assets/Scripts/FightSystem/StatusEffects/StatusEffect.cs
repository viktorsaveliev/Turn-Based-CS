using System;
using UnityEngine;

namespace Echobay.FightSystem.StatusEffects
{
    [Serializable]
    public abstract class StatusEffect
    {
        public event Action<StatusEffect> OnExpired;

        protected StatusEffectableObject StatusObject { get; private set; }

        public virtual void OnApply(StatusEffectableObject statusObject)
        { 
            StatusObject = statusObject;
        }

        public virtual void OnTurnStart() { }
        public virtual void OnTakeDamage(StatusEffectableObject attacker) { }
        public virtual void OnTurnEnd() { }
        public virtual void OnExpire() 
        {
            OnExpired?.Invoke(this);
        }
    }
}