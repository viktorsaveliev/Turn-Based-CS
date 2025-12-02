using UnityEngine;
using static Echobay.Contexts;

namespace Echobay.FightSystem.StatusEffects
{
    public abstract class DamageableStatusEffect : StatusEffect
    {
        /*protected IDamageable Damageable { get; private set; }

        public override void OnApply(ExecuteStatusEffectContext context)
        {
            base.OnApply(context);
            Damageable = context.Executer.GetComponent<IDamageable>();
        }*/
    }
}
