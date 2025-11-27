using Cysharp.Threading.Tasks;
using Echobay.GridSystem;
using Echobay.PoolSystem;
using Echobay.UnitSystem;
using UnityEngine;

namespace Echobay.CardSystem
{
    public class AlternatingAttackAction : AttackAction
    {
        [SerializeField] private ParticleSystem _attackVFXPrefab;
        [SerializeField, Range(1, 10)] private int _vfxAmount = 1;
        [SerializeField, Range(0.1f, 5)] private float _delayBetweenAttacks = 1;

        private ObjectPool<ParticleSystem> _particles;

        public override void Enter()
        {
            base.Enter();

            if (_attackVFXPrefab != null)
            {
                _particles = new(_attackVFXPrefab, null, _vfxAmount);
                _particles.CreatePool();
            }
        }

        public override void Exit()
        {
            base.Exit();

            _particles?.Clear();
            _particles = null;
        }

        public override void Execute(ExecuteActionContext context)
        {
            Enter();
            AlternatingAttack(context);
        }

        private async void AlternatingAttack(ExecuteActionContext context)
        {
            foreach (AnimationCommand animation in AnimationCommands)
            {
                Unit unit = (Unit)context.Executer;
                animation.Apply(unit.Animator);
            }
            foreach (GridCell cell in context.TargetCells)
            {
                PlayVFX(cell);

                if (cell.Occupant == null)
                {
                    await UniTask.WaitForSeconds(_delayBetweenAttacks);
                    continue;
                }

                ApplyDamage(cell);

                await UniTask.WaitForSeconds(_delayBetweenAttacks);
            }

            OnExecuted(context);
        }

        private async void PlayVFX(GridCell cell)
        {
            ParticleSystem vfx = _particles.GetInactiveObject();
            vfx.transform.position = cell.transform.position;

            vfx.gameObject.SetActive(true);
            vfx.Play();

            await UniTask.WaitForSeconds(vfx.main.duration);

            if (vfx != null)
            {
                vfx.gameObject.SetActive(false);
            }
        }
    }
}
