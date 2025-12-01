using Cysharp.Threading.Tasks;
using Echobay.CardSystem;
using Echobay.GridSystem;
using System;
using UnityEngine;
using static Echobay.Contexts;

namespace Echobay.FightSystem.StatusEffects
{
    public class HealingEffect : DamageableStatusEffect
    {
        [SerializeField, Range(1, 50)] private int _healPerTurn = 1;

        public override void OnApply(StatusEffectableObject statusObject)
        {
            base.OnApply(statusObject);
            Debug.Log($"{StatusObject.name} effect heal на {_healPerTurn} HP {RemainingTurns}");
        }

        public override async UniTask OnTurnStart(ExecuteStatusEffectContext statusEffectContext)
        {
            ICellOccupant cellOccupant = (ICellOccupant)Damageable;
            ExecuteActionContext context = new(Action, cellOccupant, cellOccupant.CurrentCell, healing: _healPerTurn);

            await ExecuteAction(context);
            await base.OnTurnStart(statusEffectContext);

            Debug.Log($"{StatusObject.name} полечился на {_healPerTurn} HP");
        }

        protected override void OnExpire()
        {
            base.OnExpire();
            Debug.Log($"{StatusObject.name} перестал лечиться");
        }
    }

}
