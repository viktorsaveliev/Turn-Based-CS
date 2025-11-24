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

        [Inject]
        public ServerActionHandler(IGrid grid, UnitSpawner spawner, CardsDatabase cardsDb, TurnController turnController)
        {
            _grid = grid;
            _spawner = spawner;
            _cardsDb = cardsDb;
            _turnController = turnController;
        }

        public bool HandleAction(NetworkExecuteActionContext networkContext, Vector2Int[] cellPositions)
        {
            if (!_spawner.TryGetUnitByID(networkContext.UnitID, out Unit unit)) return false;
            if(!_cardsDb.TryGetCardDataByID(networkContext.CardID, out CardData cardData)) return false;

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
                Debug.Log("Can't execute");
                return false;
            }

            MatchPlayer player = unit.Owner;
            int requiredPoints = cardData.RequiredActionPoints;

            if (_turnController.CurrentPlayer != player)
            {
                Debug.LogError($"Not your turn {player.Data.Name}");
                return false;
            }

            if (!_turnController.TrySpendPoints(player, requiredPoints))
            {
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

        public bool HandleMove(int unitId, Vector2Int targetCell)
        {
            if (!_spawner.TryGetUnitByID(unitId, out Unit unit))
                return false;

            if (!_grid.TryGetCellByPosition(targetCell, out GridCell cell))
                return false;

            if (!CanMove(unit, cell))
                return false;

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
