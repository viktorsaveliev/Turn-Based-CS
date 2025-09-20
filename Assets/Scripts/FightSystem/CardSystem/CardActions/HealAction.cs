using Echobay.FightSystem;
using Echobay.GridSystem;
using System;
using UnityEngine;

namespace Echobay.CardSystem
{
    public class HealAction : CardAction
    {
        public override int Value => _healAmount;

        [SerializeField, Range(1, 500)] private int _healAmount = 10;

        public override void Execute(ExecuteActionContext context)
        {
            foreach (GridCell cell in context.TargetCells)
            {
                IDamageable damageable = cell.Occupant as IDamageable;
                damageable.Health.RecoveryHealth(_healAmount);
            }
            
            OnExecuted();
        }

        public override bool CanExecute(ExecuteActionContext context)
        {
            return true;
        }
    }
}
