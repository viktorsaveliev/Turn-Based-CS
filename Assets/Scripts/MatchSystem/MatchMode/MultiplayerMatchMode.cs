using Echobay.PlayerSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.MatchSystem
{
    public class MultiplayerMatchMode : IMatchMode
    {
        private readonly List<PlayerConfig> _playerConfigs;

        public MultiplayerMatchMode(List<PlayerConfig> configs)
        {
            _playerConfigs = configs;
        }

        public void SetupPlayers(MatchController matchController)
        {
            foreach (PlayerConfig config in _playerConfigs)
            {
                MatchPlayer player = matchController.CreatePlayer(config);
                matchController.SpawnUnits(player, config);
            }

            matchController.StartTurns();
        }
    }

}
