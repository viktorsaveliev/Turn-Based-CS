using Echobay.NetworkSystem.Match;
using Echobay.PlayerSystem;
using Fusion;
using UnityEngine;
using Zenject;

namespace Echobay.UISystem
{
    public class TurnHud : PanelUI
    {
        private NetworkTurnController _networkTurnController;
        private NetworkMatchController _networkMatchController;

        [Inject]
        public void Construct(
            NetworkTurnController netTurns,
            NetworkMatchController matchInfo)
        {
            _networkTurnController = netTurns;
            _networkMatchController = matchInfo;
        }

        private void Awake()
        {
            _networkTurnController.OnCurrentPlayerChanged += HandlePlayerChanged;
        }

        private void OnDestroy()
        {
            _networkTurnController.OnCurrentPlayerChanged -= HandlePlayerChanged;
        }

        private void HandlePlayerChanged(PlayerRef newPlayer)
        {
            MatchPlayer localPlayer = _networkMatchController.LocalPlayer;

            if (localPlayer != null && newPlayer == localPlayer.Data.PlayerRef)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }
}
