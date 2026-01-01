using System;
using UnityEngine;

namespace Echobay.FightSystem
{
    public class DamageableObject : MonoBehaviour, IDamageable
    {
        public bool IsAlive { get; private set; }
        public int CurrentHealth => _health;
        public int MaxHealth => _maxHealth;

        public event Action<int> OnTakedDamage;
        public event Action<int> OnRecoveryHealth;
        public event Action OnDead;

        private int _health;
        private int _maxHealth;

        public void SetHealth(int health)
        {
            if (health < 1)
            {
                Debug.LogError($"[SetMaxHealth] Value = {health}");
                return;
            }

            if (health > _maxHealth)
            {
                health = _maxHealth;
            }

            _health = health;
        }

        public void SetMaxHealth(int health)
        {
            if (health < 1)
            {
                Debug.LogError($"[SetMaxHealth] Value = {health}");
                return;
            }

            _maxHealth = health;
        }

        public virtual void TakeDamage(DamageContext context)
        {
            if (_health <= 0)
            {
                Debug.LogError("[TakeDamage] Unit is dead");
                return;
            }

            if (context.DamageValue <= 0)
            {
                Debug.LogError("[TakeDamage] Value = 0");
                return;
            }

            _health -= context.DamageValue;

            if (_health <= 0)
            {
                IsAlive = false;
                OnDead?.Invoke();
            }

            OnTakedDamage?.Invoke(context.DamageValue);
        }

        public void RecoveryHealth(int value)
        {
            if (_health <= 0)
            {
                Debug.LogError("[RecoveryHealth] Unit is dead");
                return;
            }

            if (value <= 0)
            {
                Debug.LogError("[RecoveryHealth] Value = 0");
                return;
            }

            _health += value;

            if (_health > MaxHealth)
            {
                _health = MaxHealth;
            }

            OnRecoveryHealth?.Invoke(value);
        }
    }
}
