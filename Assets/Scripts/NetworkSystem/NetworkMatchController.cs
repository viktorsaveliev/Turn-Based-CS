using Echobay.MatchSystem;
using Echobay.PlayerSystem;
using Fusion;
using UnityEngine;
using Zenject;

namespace Echobay.NetworkSystem.Match
{
    public class NetworkMatchController : NetworkBehaviour
    {
        public MatchPlayer LocalPlayer { get; private set; }

        private MatchController _matchController;

        [Inject]
        public void Construct(MatchController matchController)
        {
            _matchController = matchController;
        }

        private void OnDestroy()
        {
            _matchController.OnPlayerCreated -= OnPlayerCreated;
        }

        public override void Spawned()
        {
            if (Object.HasStateAuthority)
            {
                _matchController.OnPlayerCreated += OnPlayerCreated;
                print("match network init");
            }
            else
            {
                print("You client");
            }
        }

        public bool TryGetMatchPlayerByRef(PlayerRef playerRef, out MatchPlayer player)
        {
            player = null;

            foreach (MatchPlayer matchPlayer in _matchController.Players)
            {
                if (matchPlayer.Data.PlayerRef != playerRef) continue;
                player = matchPlayer;
                return true;
            }

            return false;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = false)]
        public void RPC_PlayerCreated(PlayerConfig playerConfig)
        {
            MatchPlayer player = _matchController.CreatePlayer(playerConfig);
            _matchController.SpawnUnits(player, playerConfig);

            if (Runner.LocalPlayer == playerConfig.PlayerRef)
            {
                LocalPlayer = player;
            }

            print($"You Client. Created Player {playerConfig.Name}");
        }

        private void OnPlayerCreated(MatchPlayer player)
        {
            if (Runner.LocalPlayer == player.Data.PlayerRef)
            {
                LocalPlayer = player;
            }

            RPC_PlayerCreated(player.Data);
            print($"sended info about {player.Data.Name}");
        }
    }
}
