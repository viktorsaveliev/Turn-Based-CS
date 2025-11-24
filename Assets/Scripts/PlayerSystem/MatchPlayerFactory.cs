using Fusion;
using System;
using UnityEngine;

namespace Echobay.PlayerSystem
{
    public class MatchPlayerFactory
    {
        public MatchPlayer CreatePlayer(PlayerConfig config)
        {
            MatchPlayer player = new(config);
            return player;
        }
    }

    [Serializable]
    public struct PlayerConfig : INetworkStruct
    {
        public PlayerRef PlayerRef;
        public NetworkString<_16> Name;
        public int TeamID;

        [Networked, Capacity(3)]
        public NetworkArray<int> UnitsDataID => default;
    }
}
