using Echobay.CardSystem;
using Echobay.UnitSystem;
using System;
using UnityEngine;

namespace Echobay.FightSystem.StatusEffects
{
    public class CounterattackEffect : TemporaryStatusEffect
    {
        [field: SerializeReference] public CardAction Action { get; private set; }

        public override void OnTakeDamage(StatusEffectableObject attacker)
        {
            Unit attackerUnit = attacker as Unit;
            Unit unit = (Unit)StatusObject;

            ExecuteActionContext context = new(Action, unit, attackerUnit.CurrentCell);
            Action.Execute(context);
        }

        public override void OnExpire()
        {
            Debug.Log($"{StatusObject.name} контратака закончилась");
        }
    }
}