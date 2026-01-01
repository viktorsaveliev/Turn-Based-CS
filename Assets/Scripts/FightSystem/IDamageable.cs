using Echobay.FightSystem.DamageType;
using System;
using UnityEngine;

namespace Echobay.FightSystem
{
    public interface IDamageable
    {
        public bool IsAlive { get; }
        public int CurrentHealth { get; }
        public int MaxHealth { get; }

        public event Action<int> OnTakedDamage;
        public event Action<int> OnRecoveryHealth;
        public event Action OnDead;

        public void RecoveryHealth(int health);
        public void TakeDamage(DamageContext context);
    }

    public struct DamageContext
    {
        public int DamageValue;
        public DamageTypeData DamageType;
    }
}