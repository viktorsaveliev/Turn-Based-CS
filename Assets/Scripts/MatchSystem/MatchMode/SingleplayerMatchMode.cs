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
            MatchPlayer human = matchController.CreatePlayer(_humanConfig);
            MatchPlayer bot = matchController.CreatePlayer(_botConfig);

            matchController.SpawnUnits(human, _humanConfig);
            matchController.SpawnUnits(bot, _botConfig);

            //matchController.SetLocalPlayer(human);
            matchController.StartTurns();
        }
    }
}