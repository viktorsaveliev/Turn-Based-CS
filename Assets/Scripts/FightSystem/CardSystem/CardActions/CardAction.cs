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

        [SerializeField] protected AnimationCommand[] AnimationCommands;

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

        public abstract void Execute(ExecuteActionContext context);

        public virtual void OnExecuted(ExecuteActionContext context)
        {
            OnActionExecuted?.Invoke(context);
            Exit();
        }
    }
}