using System;
using UnityEngine;

namespace Echobay.FightSystem.StatusEffects
{
    public class BurningEffect : DamageableStatusEffect
    {
        [SerializeField, Range(1, 50)] private int _damagePerTurn = 1;

        public override void OnTurnStart()
        {
            base.OnTurnStart();

            if (Damageable != null)
            {
                Damageable.Health.TakeDamage(_damagePerTurn);
                Debug.Log($"{StatusObject.name} горит и получает {_damagePerTurn} урона!");
            }
        }

        public override void OnExpire()
        {
            Debug.Log($"{StatusObject.name} перестал гореть");
        }
    }

}
