using Echobay.UnitSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.PlayerSystem
{
    public class PlayerFactory
    {
        public Player CreatePlayer(PlayerConfig config)
        {
            Player player = new(config);
            return player;
        }
    }

    public struct PlayerConfig
    {
        public string Name;
        public int TeamID;
        public List<UnitData> UnitsData;

        public PlayerConfig(string name, int teamId, List<UnitData> unitDatas)
        {
            Name = name;
            TeamID = teamId;

            UnitsData = new(unitDatas);
        }
    }
}
