using Echobay.PlayerSystem;
using UnityEngine;

namespace Echobay.MatchSystem
{
    public class SingleplayerMatchMode : IMatchMode
    {
        private readonly PlayerConfig _humanConfig;
        private readonly PlayerConfig _botConfig;

        public SingleplayerMatchMode(PlayerConfig humanConfig, PlayerConfig botConfig)
        {
            _humanConfig = humanConfig;
            _botConfig = botConfig;
        }

        public void SetupPlayers(MatchController matchController)
        {
            Player human = matchController.CreatePlayer(_humanConfig);
            Player bot = matchController.CreatePlayer(_botConfig);

            matchController.SpawnUnits(human, _humanConfig);
            matchController.SpawnUnits(bot, _botConfig);

            matchController.SetLocalPlayer(human);
            matchController.StartTurns();
        }
    }
}