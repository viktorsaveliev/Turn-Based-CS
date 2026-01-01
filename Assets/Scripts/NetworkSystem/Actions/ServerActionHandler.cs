using Echobay.CardSystem;
using Echobay.GridSystem;
using Echobay.MatchSystem;
using Echobay.MatchSystem.TurnSystem;
using Echobay.PlayerSystem;
using Echobay.UnitSystem;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Echobay.NetworkSystem.Match
{
    public class ServerActionHandler
    {
        private readonly IGrid _grid;
        private readonly UnitSpawner _spawner;
        private readonly CardsDatabase _cardsDb;
        private readonly TurnController _turnController;
        private readonly GameplayData _gameplayData;

        [Inject]
        public ServerActionHandler(IGrid grid, UnitSpawner spawner, CardsDatabase cardsDb, TurnController turnController, GameplayData gameplayData)
        {
            _grid = grid;
            _spawner = spawner;
            _cardsDb = cardsDb;
            _turnController = turnController;
            _gameplayData = gameplayData;
        }

        public bool HandleAction(NetworkExecuteActionContext networkContext, Vector2Int[] cellPositions, out NetworkRejectContext rejectContext)
        {
            rejectContext = new NetworkRejectContext();

            if (!_spawner.TryGetUnitByID(networkContext.UnitID, out Unit unit))
            {
                rejectContext.ReasonText = "Target unit not found!";
                return false;
            }

            if (!_cardsDb.TryGetCardDataByID(networkContext.CardID, out CardData cardData))
            {
                rejectContext.ReasonText = "Target card not found!";
                return false;
            }

            ICardAction action = cardData.Action;

            List<GridCell> cells = new();

            foreach (Vector2Int cellPosition in cellPositions)
            {
                if (_grid.TryGetCellByPosition(cellPosition, out GridCell cell))
                {
                    cells.Add(cell);
                }
            }

            ExecuteActionContext context = new(action, unit, cells);
            if (!action.CanExecute(context))
            {
                rejectContext.ReasonText = "Can't execute!";
                return false;
            }

            MatchPlayer player = unit.Owner;
            rejectContext.PlayerRef = player.Data.PlayerRef;

            int requiredPoints = cardData.RequiredActionPoints;

            if (_turnController.CurrentPlayer != player && !context.CanWorkOnEnemyTurn)
            {
                rejectContext.ReasonText = $"Not your turn {player.Data.Name}";
                return false;
            }

            if (!_turnController.TrySpendPoints(player, requiredPoints))
            {
                rejectContext.ReasonText = "Not enough action points";
                return false;
            }

            return true;
        }

        public bool HandleAction(int unitId, IReadOnlyCollection<GridCell> cells, ICardAction action)
        {
            if (!_spawner.TryGetUnitByID(unitId, out Unit unit)) return false;

            ExecuteActionContext context = new(action, unit, cells);

            return action.CanExecute(context);
        }

        public bool HandleMove(int unitId, Vector2Int targetCell, out NetworkRejectContext rejectContext)
        {
            rejectContext = new();
            
            if (!_spawner.TryGetUnitByID(unitId, out Unit unit))
            {
                rejectContext.ReasonText = "Can't find target unit";
                return false;
            }
            else
            {
                rejectContext.PlayerRef = unit.Owner.Data.PlayerRef;
            }

            if (!_grid.TryGetCellByPosition(targetCell, out GridCell cell))
            {
                rejectContext.ReasonText = "Can't find target cell";
                return false;
            }

            if (!_turnController.TrySpendPoints(unit.Owner, _gameplayData.SpendPointsForMovement))
            {
                rejectContext.ReasonText = "Not enough action points";
                return false;
            }

            if (!CanMove(unit, cell))
            {
                rejectContext.ReasonText = "Can't move";
                return false;
            }

            return true;
        }

        private bool CanMove(Unit unit, GridCell cell)
        {
            if (cell.IsOccupied) return false;
            // ... another requier
            return true;
        }
    }

}
