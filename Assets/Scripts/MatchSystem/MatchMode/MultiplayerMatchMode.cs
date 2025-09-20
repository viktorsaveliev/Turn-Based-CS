using Echobay.PlayerSystem;
using UnityEngine;

namespace Echobay.MatchSystem
{
    public class MultiplayerMatchMode : IMatchMode
    {
        private readonly PlayerConfig[] _playerConfigs;

        public MultiplayerMatchMode(PlayerConfig[] configs)
        {
            _playerConfigs = configs;
        }

        public void SetupPlayers(MatchController matchController)
        {
            foreach (PlayerConfig config in _playerConfigs)
            {
                Player player = matchController.CreatePlayer(config);
                matchController.SpawnUnits(player, config);
            }

            matchController.StartTurns();
        }
    }

}
