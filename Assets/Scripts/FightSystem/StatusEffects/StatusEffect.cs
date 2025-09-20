using System;
using UnityEngine;

namespace Echobay.FightSystem.StatusEffects
{
    [Serializable]
    public abstract class StatusEffect
    {
        protected StatusEffectableObject StatusObject { get; private set; }

        public virtual void OnApply(StatusEffectableObject statusObject)
        { 
            StatusObject = statusObject;
            Debug.Log(StatusObject.name); 
        }

        public virtual void OnTurnStart() { }
        public virtual void OnTakeDamage(StatusEffectableObject attacker) { }
        public virtual void OnTurnEnd() { }
        public virtual void OnExpire() { }
    }
}