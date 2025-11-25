using Echobay.ActionContext;
using Echobay.CardSystem;
using Echobay.GridSystem;
using Echobay.MatchSystem;
using Echobay.PlayerSystem;
using Echobay.UnitSystem;
using System;
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
        private readonly NetworkMatchController _networkMatchController;

        [Inject]
        public ClientActionExecutor(
            ActionController actionController,
            UnitSpawner spawner,
            IPathFinder pathFinder,
            IGrid grid,
            CardsDatabase cardsDb,
            NetworkMatchController networkMatchController)
        {
            _actionController = actionController;
            _spawner = spawner;
            _pathFinder = pathFinder;
            _grid = grid;
            _cardsDb = cardsDb;
            _networkMatchController = networkMatchController;
        }

        public void ExecuteAction(int unitId, Vector2Int[] cellPositions, int cardID)
        {
            if (!_cardsDb.TryGetCardDataByID(cardID, out CardData cardData))
                return;

            var cells = ResolveCells(cellPositions);

            ExecuteCardAction(unitId, cardData.Action, cells);
        }

        public void ExecuteAction(int unitId, IReadOnlyCollection<GridCell> cells, ICardAction action)
        {
            ExecuteCardAction(unitId, action, cells);
        }

        public void ExecuteMove(int unitId, Vector2Int targetCellPosition)
        {
            if (!TryGetUnit(unitId, out Unit unit))
                return;

            if (!_grid.TryGetCellByPosition(targetCellPosition, out GridCell cell))
                return;

            MoveAction action = new(_pathFinder);

            var context = new ExecuteActionContext(action, unit, cell);
            action.Execute(context);

            RunOnLocalOwnedUnit(unit, () =>
            {
                void OnCompleted()
                {
                    unit.OnPathCompleted -= OnCompleted;
                    _actionController.CancelAction();
                }

                unit.OnPathCompleted += OnCompleted;
            });
        }

        private void ExecuteCardAction(int unitId, ICardAction action, IReadOnlyCollection<GridCell> cells)
        {
            if (!TryGetUnit(unitId, out Unit unit))
                return;

            var context = new ExecuteActionContext(action, unit, cells);
            action.Execute(context);

            RunOnLocalOwnedUnit(unit, () =>
            {
                void OnCompleted()
                {
                    action.OnActionExecuted -= OnCompleted;
                    _actionController.ActionExecuted();
                }

                action.OnActionExecuted += OnCompleted;
            });
        }

        private bool TryGetUnit(int unitId, out Unit unit)
        {
            return _spawner.TryGetUnitByID(unitId, out unit);
        }

        private List<GridCell> ResolveCells(Vector2Int[] positions)
        {
            var result = new List<GridCell>(positions.Length);

            foreach (var pos in positions)
            {
                if (_grid.TryGetCellByPosition(pos, out GridCell cell))
                    result.Add(cell);
            }

            return result;
        }

        private void RunOnLocalOwnedUnit(Unit unit, Action callback)
        {
            MatchPlayer localPlayer = _networkMatchController.LocalPlayer;
            if (localPlayer == unit.Owner && localPlayer.ActionPoints > 0)
                callback?.Invoke();
        }
    }
}
