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
            if (statusEffectContext.Executer != null)
            {
                ICellOccupant cellOccupant = (ICellOccupant)statusEffectContext.Executer;

                ExecuteActionContext context = new(Data.Action, cellOccupant, cellOccupant.CurrentCell, Data.Action.Value)
                {
                    Token = statusEffectContext.Token
                };

                await ExecuteAction(context);
            }

            await base.OnTurnEnd(statusEffectContext);
        }
    }
}
