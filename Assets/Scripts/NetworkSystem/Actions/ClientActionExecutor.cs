using Cysharp.Threading.Tasks;
using Echobay.ActionContext;
using Echobay.CardSystem;
using Echobay.FightSystem.StatusEffects;
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

            ExecuteCardAction(unitId, cardData, cells).Forget();
        }

        public void ExecuteMove(int unitId, Vector2Int targetCellPosition)
        {
            if (!TryGetUnit(unitId, out Unit unit))
                return;

            if (!_grid.TryGetCellByPosition(targetCellPosition, out GridCell cell))
                return;

            MoveAction action = new(_pathFinder);

            ExecuteActionContext context = new(action, unit, cell);
            action.Execute(context).Forget();

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

        private async UniTaskVoid ExecuteCardAction(int unitId, CardData cardData, IReadOnlyCollection<GridCell> cells)
        {
            if (!TryGetUnit(unitId, out Unit unit)) return;

            ExecuteActionContext context = new(cardData.Action, unit, cells);
            CardAction cardAction = cardData.Action;

            await cardAction.Execute(context);

            ApplyEffects(cardData, context);

            RunOnLocalOwnedUnit(unit, () =>
            {
                _actionController.ActionExecuted(context);
                /*void OnCompleted(ExecuteActionContext context)
                {
                    Debug.Log("1");
                    action.OnActionExecuted -= OnCompleted;
                    _actionController.ActionExecuted(context);
                }

                action.OnActionExecuted += OnCompleted;*/
            });
        }

        private void ApplyEffects(CardData cardData, ExecuteActionContext context)
        {
            foreach (StatusEffectData effectData in cardData.EffectsForAttacker)
            {
                Unit unit = (Unit)context.Executer;
                unit.AddEffect(effectData);
            }

            foreach (GridCell cell in context.TargetCells)
            {
                if (cell.Occupant is Unit unit)
                {
                    foreach (StatusEffectData effectData in cardData.EffectsForTarget)
                    {
                        unit.AddEffect(effectData);
                    }
                }
            }
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
            if (localPlayer == unit.Owner) //  && localPlayer.ActionPoints > 0
            {
                callback?.Invoke();
            }
        }
    }
}
