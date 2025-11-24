using Echobay.ActionContext;
using Echobay.CardSystem;
using Echobay.GridSystem;
using Echobay.UnitSystem;
using Fusion;
using UnityEngine;
using Zenject;

namespace Echobay.NetworkSystem.Match
{
    public class NetworkActionController : NetworkBehaviour
    {
        private ActionController _action;
        private ServerActionHandler _server;
        private ClientActionExecutor _client;
        private CardsDatabase _cardsDatabase;

        [Inject]
        public void Construct(
            ActionController action, 
            ServerActionHandler server, 
            ClientActionExecutor client,
            CardsDatabase cardsDatabase)
        {
            _action = action;
            _server = server;
            _client = client;
            _cardsDatabase = cardsDatabase;
        }

        private void OnEnable()
        {
            _action.OnMoveRequested += OnMoveRequested;
            _action.OnActionRequested += OnActionRequested;
        }

        private void OnDisable()
        {
            _action.OnMoveRequested -= OnMoveRequested;
            _action.OnActionRequested -= OnActionRequested;
        }

        #region Action Request
        private void OnActionRequested(CardData cardData, ExecuteActionContext context)
        {
            if (Runner.IsSinglePlayer)
            {
                ProcessActionLocally(context);
            }
            else
            {
                NetworkExecuteActionContext networkContext = ConvertLocalToNetworkContext(cardData, context);
                RPC_RequestAction(networkContext);
            }
        }

        private void ProcessActionLocally(ExecuteActionContext context)
        {
            Unit unit = (Unit)context.Executer;

            if (_server.HandleAction(unit.UnitID, context.TargetCells, context.Action))
            {
                _client.ExecuteAction(unit.UnitID, context.TargetCells, context.Action);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_RequestAction(NetworkExecuteActionContext context)
        {
            if (!Object.HasStateAuthority) return;

            Vector2Int[] targetCellPositions = ConvertNetworkArrayToArray(context);

            if (_server.HandleAction(context, targetCellPositions))
            {
                RPC_ApplyAction(context);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ApplyAction(NetworkExecuteActionContext context)
        {
            Vector2Int[] targetCellPositions = ConvertNetworkArrayToArray(context);
            _client.ExecuteAction(context.UnitID, targetCellPositions, context.CardID);
        }
        
        private Vector2Int[] ConvertNetworkArrayToArray(NetworkExecuteActionContext context)
        {
            Vector2Int[] array = new Vector2Int[context.TargetsCount];
            context.TargetCells.CopyTo(array);

            return array;
        }

        private NetworkExecuteActionContext ConvertLocalToNetworkContext(CardData cardData, ExecuteActionContext context)
        {
            int cardID = 0;

            if (_cardsDatabase.TryGetCardID(cardData, out int cardDataID))
            {
                cardID = cardDataID;
            }
            else
            {
                Debug.LogError($"Card {cardData.Name} not found in database");
            }

            Unit unit = (Unit)context.Executer;
            NetworkExecuteActionContext networkContext = new()
            {
                PlayerRef = unit.Owner.Data.PlayerRef,
                UnitID = unit.UnitID,
                CardID = cardID
            };

            Vector2Int[] TargetCellPositions = new Vector2Int[context.TargetCells.Count];

            for (int i = 0; i < TargetCellPositions.Length; i++)
            {
                TargetCellPositions[i] = context.TargetCells[i].Position;
            }

            networkContext.TargetCells.CopyFrom(TargetCellPositions, 0, context.TargetCells.Count);
            networkContext.TargetsCount = context.TargetCells.Count;

            return networkContext;
        }
        #endregion

        #region Move Request
        private void RequestMove(int unitId, Vector2Int target)
        {
            if (Runner.IsSinglePlayer)
            {
                ProcessMoveLocally(unitId, target);
            }
            else
            {
                RPC_RequestMove(unitId, target);
            }
        }

        private void OnMoveRequested(IUnitCellOccupant cellOccupant, GridCell targetCell)
        {
            if (cellOccupant is not Unit unit)
            {
                Debug.LogError($"Move request: target occupant is not a Unit ({cellOccupant})");
                return;
            }

            RequestMove(unit.UnitID, targetCell.Position);
        }

        private void ProcessMoveLocally(int unitId, Vector2Int target)
        {
            if (_server.HandleMove(unitId, target))
            {
                _client.ExecuteMove(unitId, target);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_RequestMove(int unitId, Vector2Int target)
        {
            if (!Object.HasStateAuthority) return;

            if (_server.HandleMove(unitId, target))
            {
                RPC_ApplyMove(unitId, target);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ApplyMove(int unitId, Vector2Int target)
        {
            _client.ExecuteMove(unitId, target);
        }
        #endregion
    }

    public struct NetworkExecuteActionContext : INetworkStruct
    {
        public PlayerRef PlayerRef;
        public int CardID;
        public int UnitID;
        public int TargetsCount;

        [Networked, Capacity(10)]
        public NetworkArray<Vector2Int> TargetCells => default;
    }
}
