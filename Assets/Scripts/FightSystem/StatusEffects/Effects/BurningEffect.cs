using Cysharp.Threading.Tasks;
using Echobay.CardSystem;
using Echobay.GridSystem;
using System;
using UnityEngine;
using static Echobay.Contexts;

namespace Echobay.FightSystem.StatusEffects
{
    public class BurningEffect : DamageableStatusEffect
    {
        public override async UniTask OnTurnEnd(ExecuteStatusEffectContext statusEffectContext)
        {
            if (Damageable != null)
            {
                ICellOccupant cellOccupant = (ICellOccupant)Damageable;

                ExecuteActionContext context = new(Action, cellOccupant, cellOccupant.CurrentCell, Action.Value)
                {
                    Token = statusEffectContext.Token
                };

                await ExecuteAction(context);
            }

            await base.OnTurnEnd(statusEffectContext);
        }

        protected override void OnExpire()
        {
            base.OnExpire();
            Debug.Log($"{StatusObject.name} перестал гореть");
        }
    }
}
