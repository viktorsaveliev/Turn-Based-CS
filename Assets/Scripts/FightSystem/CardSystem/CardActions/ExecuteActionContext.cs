using Echobay.GridSystem;
using System.Collections.Generic;
using System.Threading;

namespace Echobay.CardSystem
{
    public struct ExecuteActionContext
    {
        public ICardAction Action { get; set; }
        public ICellOccupant Executer { get; set; }
        public List<GridCell> TargetCells { get; set; }
        public readonly GridCell TargetCell => TargetCells[0];
        public int Damage { get; set; }
        public int Healing { get; set; }
        public bool CanWorkOnEnemyTurn { get; set; }

        public CancellationToken Token { get; set; }

        public ExecuteActionContext(ICardAction cardAction, ICellOccupant executer, GridCell targetCell, int damage = 0, int healing = 0)
        {
            Action = cardAction;
            Executer = executer;

            TargetCells = new()
            {
                targetCell
            };

            Damage = damage;
            Healing = healing;
            Token = default;

            CanWorkOnEnemyTurn = false;
        }

        public ExecuteActionContext(ICardAction cardAction, ICellOccupant executer, IReadOnlyCollection<GridCell> targetCells, int damage = 0, int healing = 0)
        {
            Action = cardAction;
            Executer = executer;

            TargetCells = new();
            foreach (GridCell cell in targetCells)
            {
                TargetCells.Add(cell);
            }

            Damage = damage;
            Healing = healing;
            Token = default;

            CanWorkOnEnemyTurn = false;
        }
    }
}
