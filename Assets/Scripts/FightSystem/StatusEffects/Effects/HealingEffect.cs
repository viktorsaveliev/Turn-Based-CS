using Cysharp.Threading.Tasks;
using Echobay.CardSystem;
using Echobay.GridSystem;
using static Echobay.Contexts;

namespace Echobay.FightSystem.StatusEffects
{
    public class HealingEffect : DamageableStatusEffect
    {
        public override async UniTask OnTurnStart(ExecuteStatusEffectContext statusEffectContext)
        {
            ICellOccupant cellOccupant = (ICellOccupant)statusEffectContext.Executer;
            ExecuteActionContext context = new(Data.Action, cellOccupant, cellOccupant.CurrentCell, healing: Data.Action.Value);

            await ExecuteAction(context);
            await base.OnTurnStart(statusEffectContext);
        }
    }
}
