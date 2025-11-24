using Echobay.MatchSystem;
using Echobay.PlayerSystem;
using Fusion;
using UnityEngine;
using Zenject;

namespace Echobay.NetworkSystem.Match
{
    public class NetworkMatchMediator : NetworkBehaviour
    {
        private MatchController _match;
        private NetworkRunnerProvider _network;

        [Inject]
        public void Construct(MatchController match, NetworkRunnerProvider networkRunnerProvider)
        {
            _match = match;
            _network = networkRunnerProvider;
        }

        public override void Spawned()
        {
            if (Object.HasStateAuthority)
            {
                StartMatchOnServer();
            }
        }

        private void StartMatchOnServer()
        {
            var players = _network.RoomController.Players;
            _match.StartMultiplayerMatch(players);

            RPC_MatchStarted();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_MatchStarted()
        {
            Debug.Log("Match started for client!");
        }
    }
}
