using System;

namespace Echobay
{
    public class HealthSystem
    {
        public bool IsAlive { get; private set; }
        public int Value => _health;
        public int MaxHealth => _maxHealth;

        public event Action<int> OnTakedDamage;
        public event Action<int> OnRecoveryHealth;
        public event Action OnDead;

        private int _health;
        private int _maxHealth;

        public HealthSystem(int maxHealth)
        {
            _maxHealth = maxHealth;
            _health = maxHealth;

            IsAlive = true;
        }

        public void SetHealth(int health)
        {
            if (health < 1 || health > _maxHealth)
            {
                throw new ArgumentException();
            }

            _health = health;
        }

        public void SetMaxHealth(int health)
        {
            if (health < 1)
            {
                throw new ArgumentException();
            }

            _maxHealth = health;
        }

        public void TakeDamage(int damage)
        {
            if (_health <= 0) return;

            _health -= damage;

            if (_health <= 0)
            {
                IsAlive = false;
                OnDead?.Invoke();
            }

            OnTakedDamage?.Invoke(damage);
        }

        public void RecoveryHealth(int value)
        {
            if (_health <= 0) return;

            _health += value;

            if (_health <= 0)
            {
                IsAlive = false;
                OnDead?.Invoke();
            }

            OnRecoveryHealth?.Invoke(value);
        }
    }
}