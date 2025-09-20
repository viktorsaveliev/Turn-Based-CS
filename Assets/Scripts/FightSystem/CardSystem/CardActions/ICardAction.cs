using System;

namespace Echobay.CardSystem
{
    public interface ICardAction
    {
        public event Action OnActionExecuted;

        public void Enter();
        public void Exit();
        public bool CanExecute(ExecuteActionContext context);
        public void Execute(ExecuteActionContext context);
        public void OnExecuted();
    }
}