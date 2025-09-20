using UnityEngine;

namespace Echobay.FightSystem.StatusEffects
{
    public class DamageableStatusEffect : TemporaryStatusEffect
    {
        protected IDamageable Damageable { get; private set; }

        public override void OnApply(StatusEffectableObject statusObject)
        {
            base.OnApply(statusObject);
            Damageable = statusObject.GetComponent<IDamageable>();
        }
    }
}
