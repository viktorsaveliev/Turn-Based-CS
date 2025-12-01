using Cysharp.Threading.Tasks;
using Echobay.CardSystem;
using Echobay.UnitSystem;
using UnityEngine;
using static Echobay.Contexts;

namespace Echobay.FightSystem.StatusEffects
{
    public class CounterattackEffect : TemporaryStatusEffect
    {
        public override async UniTask OnTakeDamage(ExecuteStatusEffectContext statusEffectContext)
        {
            Unit attackerUnit = statusEffectContext.Attacker as Unit;
            Unit unit = (Unit)StatusObject;

            ExecuteActionContext context = new(Action, unit, attackerUnit.CurrentCell);
            await ExecuteAction(context);

            await base.OnTakeDamage(statusEffectContext);
        }

        protected override void OnExpire()
        {
            base.OnExpire();
            Debug.Log($"{StatusObject.name} контратака закончилась");
        }
    }
}