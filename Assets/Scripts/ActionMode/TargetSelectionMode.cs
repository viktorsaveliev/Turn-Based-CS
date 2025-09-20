using Echobay.GridSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.ActionContext
{
    [Serializable]
    public abstract class TargetSelectionMode
    {
        public event Action<IReadOnlyCollection<GridCell>> OnCompleted;
        public event Action OnUpdated;

        protected ActionContextLinks ContextLinks { get; private set; }
        protected List<GridCell> CellList = new();

        public virtual void Enter(ActionContextLinks contextLinks)
        {
            ContextLinks = contextLinks;
        }

        public virtual void Exit()
        {

        }

        public abstract void HandleCellClick(GridCell cell);
        public abstract void OnCellEnter(GridCell cell);
        public abstract void OnCellExit(GridCell cell);
        public abstract void OnClickOnCard();
        public abstract string GetDescription();
        protected void Update()
        {
            OnUpdated?.Invoke();
        }
        protected void Complete()
        {
            OnCompleted?.Invoke(CellList);
        }
    }
}
