using Cysharp.Threading.Tasks;
using Echobay.UnitSystem;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Echobay.CardSystem
{
    [Serializable]
    public abstract class CardAction : ICardAction
    {
        public event Action<ExecuteActionContext> OnActionExecuted;

        public abstract int Value { get; }

        [field: SerializeField, ReadOnly] public string Key { get; private set; }

        [Header("Animation Settings")]
        [SerializeField] private AnimationCommand[] _animationCommands;

        [SerializeField, Range(0, 10f), Tooltip("delay for synchronization with animation")] 
        private float _executionDelay = 0.3f;

        [SerializeField, Range(0, 10f)] private float _postDelay = 0.2f;

        public CardAction()
        {
            Key = GetType().Name;
        }

        public virtual void Enter()
        {

        }

        public virtual void Exit()
        {

        }

        public abstract bool CanExecute(ExecuteActionContext context);

        public virtual async UniTask Execute(ExecuteActionContext context)
        {
            Unit unit = (Unit)context.Executer;

            unit.PlayAnimation(_animationCommands);

            if (_executionDelay > 0)
            {
                await UniTask.WaitForSeconds(_executionDelay, cancellationToken: context.Token);
            }

            await ExecuteLogic(context);

            if (_postDelay > 0)
            {
                await UniTask.WaitForSeconds(_postDelay, cancellationToken: context.Token);
            }

            OnActionExecuted?.Invoke(context);
        }

        protected abstract UniTask ExecuteLogic(ExecuteActionContext context);
    }
}