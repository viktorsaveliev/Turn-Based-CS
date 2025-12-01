using Echobay.CardSystem;
using UnityEngine;

namespace Echobay.FightSystem.StatusEffects
{
    public abstract class TemporaryStatusEffect : StatusEffect
    {
        [field: SerializeField, Range(1, 5)] public int RemainingTurns { get; private set; } = 1;

        protected override void OnActionExecuted(ExecuteActionContext context)
        {
            base.OnActionExecuted(context);
            Tick();
        }

        private bool Tick()
        {
            RemainingTurns--;

            if (RemainingTurns <= 0)
            {
                OnExpire();
                return true;
            }

            return false;
        }
    }
}