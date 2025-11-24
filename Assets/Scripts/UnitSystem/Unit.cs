using Echobay.FightSystem;
using Echobay.FightSystem.StatusEffects;
using Echobay.GridSystem;
using Echobay.PlayerSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Echobay.UnitSystem
{
    public abstract class Unit : StatusEffectableObject, IUnit, IUnitCellOccupant, IDamageable
    {
        public GridCell CurrentCell { get; set; }

        public HealthSystem Health { get; private set; }
        public MatchPlayer Owner { get; private set; }

        public event Action OnPathCompleted;

        [field: SerializeField, ReadOnly] public int UnitID { get; private set; }
        [field: SerializeField] public int TeamID { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }

        [SerializeField] private UnitData _data;

        private readonly HashSet<IUnitSystem> _systems = new();
        private Coroutine _moveRoutine;

        public virtual void Init(MatchPlayer owner, int unitID)
        {
            Owner = owner;
            UnitID = unitID;
            TeamID = owner.Data.TeamID;

            Health = new HealthSystem(_data.MaxHealth);
        }

        public void MoveAlongPath(List<GridCell> path)
        {
            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
            }

            _moveRoutine = StartCoroutine(MoveRoutine(path));
        }

        #region System Management
        public void AddSystem(IUnitSystem system)
        {
            _systems.Add(system);
        }

        public void RemoveSystem(IUnitSystem system)
        {
            _systems.Remove(system);
        }

        public A GetSystem<A>() where A : IUnitSystem
        {
            return _systems.OfType<A>().FirstOrDefault();
        }
        #endregion

        private IEnumerator MoveRoutine(List<GridCell> path)
        {
            foreach (var cell in path)
            {
                Vector3 target = cell.transform.position;

                while (Vector3.Distance(transform.position, target) > 0.05f)
                {
                    Vector3 direction = (target - transform.position).normalized;
                    transform.position += _data.MoveSpeed * Time.deltaTime * direction;
                    yield return null;
                }

                transform.position = target;
                CurrentCell = cell;

                yield return null;
            }

            _moveRoutine = null;
            OnPathCompleted?.Invoke();
        }

        public T GetData<T>() where T : UnitData => _data as T;
    }
}