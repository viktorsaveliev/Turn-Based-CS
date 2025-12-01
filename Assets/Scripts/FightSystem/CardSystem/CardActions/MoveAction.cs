using Cysharp.Threading.Tasks;
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

        protected override async UniTask ExecuteLogic(ExecuteActionContext context)
        {
            /*using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                   context.Cancellation,
                   StatusObject.DestroyToken);*/

            if (context.Executer is ICellMoveableOccupant moveableOccupant)
            {
                GridCell currentCell = moveableOccupant.CurrentCell;
                List<GridCell> path = _pathFinder.FindPath(currentCell, context.TargetCell);

                if (path != null && path.Count > 0)
                {
                    currentCell.SetOccupant(null);

                    var tcs = new UniTaskCompletionSource();

                    void OnPathCompleted()
                    {
                        moveableOccupant.OnPathCompleted -= OnPathCompleted;
                        tcs.TrySetResult();
                    }

                    moveableOccupant.OnPathCompleted += OnPathCompleted;

                    moveableOccupant.MoveAlongPath(path);

                    await tcs.Task; // .AttachExternalCancellation(linkedCts.Token)

                    context.TargetCell.SetOccupant(context.Executer);
                }
            }
        }

        public override bool CanExecute(ExecuteActionContext context)
        {
            return !context.TargetCell.IsOccupied; //  && !context.TargetCell.IsInteractable
        }
    }
}
