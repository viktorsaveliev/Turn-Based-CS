using Echobay.FightSystem;
using Echobay.GridSystem;
using System;
using UnityEngine;

namespace Echobay.CardSystem
{
    public abstract class AttackAction : CardAction
    {
        public override int Value => DamageAmount;

        [SerializeField, Range(1, 500)] protected int DamageAmount = 10;

        public override bool CanExecute(ExecuteActionContext context)
        {
            return true;
        }

        protected void ApplyDamage(GridCell cell)
        {
            if (cell.Occupant is IDamageable damageable)
            {
                damageable.Health.TakeDamage(DamageAmount);
            }
        }
    }
}