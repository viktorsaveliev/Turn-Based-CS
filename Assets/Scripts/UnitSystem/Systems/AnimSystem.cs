using UnityEngine;

namespace Echobay.UnitSystem
{
    public class AnimSystem : IUnitSystem
    {
        private readonly Animator _animator;

        private readonly int _moveAnimHash;
        private readonly int _attackAnimHash;
        private readonly int _deathAnimHash;

        public AnimSystem(Unit targetUnit)
        {
            Unit unit = targetUnit;
            _animator = unit.GetComponent<Animator>();

            _moveAnimHash = Animator.StringToHash("Vertical");
            _attackAnimHash = Animator.StringToHash("Attack");
            _deathAnimHash = Animator.StringToHash("Death");
        }

        public void Move()
        {
            _animator.SetFloat(_moveAnimHash, 2);
        }

        public void Stop()
        {
            _animator.SetFloat(_moveAnimHash, 0);
        }

        public void Attack()
        {
            _animator.SetTrigger(_attackAnimHash);
        }

        public void Death()
        {
            _animator.SetTrigger(_deathAnimHash);
        }
    }
}