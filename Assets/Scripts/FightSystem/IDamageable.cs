using UnityEngine;

namespace Echobay.FightSystem
{
    public interface IDamageable
    {
        public HealthSystem Health { get; }
    }
}