using Echobay.ActionContext;
using Echobay.NetworkSystem.Match;
using Fusion;
using System;
using UnityEngine;
using Zenject;

namespace Echobay.MatchSystem.TurnSystem
{
    public class GridTurnActivator : IDisposable
    {
        private NetworkTurnController _networkTurnController;
        private NetworkMatchController _networkMatchController;
        private ActionController _actionController;

        [Inject]
        public void Construct(
            NetworkTurnController netTurns,
            NetworkMatchController matchInfo,
            ActionController actionController)
        {
            _networkTurnController = netTurns;
            _networkMatchController = matchInfo;
            _actionController = actionController;
        }

        public void Init()
        {
            _networkTurnController.OnCurrentPlayerChanged += HandlePlayerChanged;
        }

        public void Dispose()
        {
            _networkTurnController.OnCurrentPlayerChanged -= HandlePlayerChanged;
        }

        private void HandlePlayerChanged(PlayerRef newPlayer)
        {
            if (newPlayer == _networkMatchController.LocalPlayer.Data.PlayerRef)
            {
                _actionController.SelecttCellAction();
            }
            else
            {
                _actionController.BlockActions();
            }
        }
    }
}
