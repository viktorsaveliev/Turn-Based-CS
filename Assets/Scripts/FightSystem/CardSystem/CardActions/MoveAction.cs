using Echobay.GridSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.CardSystem
{
    public class MoveAction : CardAction
    {
        public override int Value => 0;

        private readonly IPathFinder _pathFinder;

        public MoveAction(IPathFinder pathFinder)
        {
            _pathFinder = pathFinder;
        }

        public override void Execute(ExecuteActionContext context)
        {
            if (context.Executer is ICellMoveableOccupant moveableOccupant)
            {
                GridCell currentCell = moveableOccupant.CurrentCell;
                List<GridCell> path = _pathFinder.FindPath(currentCell, context.TargetCell);

                if (path != null && path.Count > 0)
                {
                    currentCell.SetOccupant(null);
                    moveableOccupant.MoveAlongPath(path);
                    context.TargetCell.SetOccupant(context.Executer);

                    moveableOccupant.OnPathCompleted += OnPathCompleted;

                    void OnPathCompleted()
                    {
                        moveableOccupant.OnPathCompleted -= OnPathCompleted;
                        OnExecuted();
                    }
                }
            }
        }

        public override bool CanExecute(ExecuteActionContext context)
        {
            return !context.TargetCell.IsOccupied; //  && !context.TargetCell.IsInteractable
        }
    }
}
