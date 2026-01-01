using Cysharp.Threading.Tasks;
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

        protected override UniTask ExecuteLogic(ExecuteActionContext context)
        {
            foreach (GridCell cell in context.TargetCells)
            {
                IDamageable damageable = cell.Occupant as IDamageable;
                damageable.RecoveryHealth(_healAmount);
            }

            return UniTask.CompletedTask;
        }

        public override bool CanExecute(ExecuteActionContext context)
        {
            return true;
        }
    }
}
