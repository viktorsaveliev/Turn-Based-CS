using System;
using UnityEngine;

namespace Echobay.FightSystem.StatusEffects
{
    public class HealingEffect : DamageableStatusEffect
    {
        [SerializeField, Range(1, 50)] private int _healPerTurn = 1;

        public override void OnTurnStart()
        {
            base.OnTurnStart();

            if (Damageable != null)
            {
                Damageable.Health.RecoveryHealth(_healPerTurn);
                Debug.Log($"{StatusObject.name} излечился на {_healPerTurn} ХП");
            }

            Tick();
        }

        public override void OnExpire()
        {
            base.OnExpire();
            Debug.Log($"{StatusObject.name} перестал лечиться");
        }
    }

}
