using System;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.UnitSystem
{
    public class AISystem : IUnitSystem
    {
        public IState CurrentState => _stateMachine.CurrentState;

        private readonly StateMachine _stateMachine = new();
        private readonly IUnit _unit;

        public AISystem(IUnit unit)
        {
            _unit = unit;
            InitStates();
        }

        #region StateMachine
        public void Pursuit()
        {
            //AttackState pursuitState = (AttackState)_stateMachine.GetState<AttackState>();
            //pursuitState.Pursuit(target);

            //_stateMachine.ChangeState(pursuitState);
        }

        private void InitStates()
        {
            _stateMachine.StateMap = new Dictionary<Type, IState>
            {
                //[typeof(AttackState)] = pursuitState
            };
        }
        #endregion
    }
}