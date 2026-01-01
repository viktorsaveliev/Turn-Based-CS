using Echobay.FightSystem;
using Echobay.FightSystem.DamageType;
using Echobay.GridSystem;
using System;
using UnityEngine;

namespace Echobay.CardSystem
{
    public abstract class AttackAction : CardAction
    {
        public override int Value => DamageAmount;

        [field: SerializeField] public DamageTypeData DamageType { get; private set; }

        [SerializeField, Range(1, 500)] protected int DamageAmount = 10;

        public override bool CanExecute(ExecuteActionContext context)
        {
            return true;
        }

        protected void ApplyDamage(GridCell cell)
        {
            if (cell.Occupant is IDamageable damageable)
            {
                DamageContext context = new()
                {
                    DamageValue = DamageAmount,
                    DamageType = DamageType
                };

                damageable.TakeDamage(context);
            }
        }
    }
}