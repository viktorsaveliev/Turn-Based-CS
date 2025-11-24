using Echobay.ActionContext;
using Echobay.CardSystem;
using Echobay.GridSystem;
using Echobay.MatchSystem;
using Echobay.UnitSystem;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Echobay.NetworkSystem.Match
{
    public class ClientActionExecutor
    {
        private readonly ActionController _actionController;
        private readonly UnitSpawner _spawner;
        private readonly IPathFinder _pathFinder;
        private readonly IGrid _grid;
        private readonly CardsDatabase _cardsDb;

        [Inject]
        public ClientActionExecutor(ActionController actionController, UnitSpawner spawner, IPathFinder pathFinder, IGrid grid, CardsDatabase cardsDb)
        {
            _actionController = actionController;
            _spawner = spawner;
            _pathFinder = pathFinder;
            _grid = grid;
            _cardsDb = cardsDb;
        }

        public void ExecuteAction(int unitId, Vector2Int[] cellPositions, int cardID)
        {
            if (!_spawner.TryGetUnitByID(unitId, out Unit unit)) return;
            if (!_cardsDb.TryGetCardDataByID(cardID, out CardData cardData)) return;

            ICardAction action = cardData.Action;

            List<GridCell> cells = new();

            foreach (Vector2Int cellPosition in cellPositions)
            {
                if (_grid.TryGetCellByPosition(cellPosition, out GridCell cell))
                {
                    cells.Add(cell);
                }
            }

            var context = new ExecuteActionContext(action, unit, cells);
            action.Execute(context);

            action.OnActionExecuted += OnCompleted;

            void OnCompleted()
            {
                action.OnActionExecuted -= OnCompleted;
                _actionController.ActionExecuted();
            }
        }

        public void ExecuteAction(int unitId, IReadOnlyCollection<GridCell> cells, ICardAction action)
        {
            if (!_spawner.TryGetUnitByID(unitId, out Unit unit)) return;

            var context = new ExecuteActionContext(action, unit, cells);
            action.Execute(context);

            action.OnActionExecuted += OnCompleted;

            void OnCompleted()
            {
                action.OnActionExecuted -= OnCompleted;
                _actionController.ActionExecuted();
            }
        }

        public void ExecuteMove(int unitId, Vector2Int targetCellPosition)
        {
            if (!_spawner.TryGetUnitByID(unitId, out Unit unit))
                return;

            if (_grid.TryGetCellByPosition(targetCellPosition, out GridCell cell))
            {
                MoveAction action = new(_pathFinder);
                action.Execute(new ExecuteActionContext(action, unit, cell));

                unit.OnPathCompleted += OnCompleted;

                void OnCompleted()
                {
                    unit.OnPathCompleted -= OnCompleted;
                    _actionController.CancelAction();
                }
            }
        }
    }
}
