using Echobay.PlayerSystem;
using System;
using UnityEngine;

namespace Echobay.NetworkSystem.Lobby.Rooms
{
    public class LocalRoomPlayer : IPlayerData
    {
        public event Action OnDataChanged;

        public string PlayerName { get; private set; }
        public int TeamID { get; private set; }
        public int[] UnitsDataID { get; private set; }

        public void Initialize(string playerName, int[] warriorDatasID)
        {
            PlayerName = playerName;
            UnitsDataID = warriorDatasID;
        }

        public void Initialize()
        {
            UnitsDataID = new int[3] { 0, 1, 2 };
        }

        public void SetUnits(int[] units)
        {
            UnitsDataID = units;
            OnDataChanged?.Invoke();
        }

        public void SetNickname(string nickname)
        {
            PlayerName = nickname;
            OnDataChanged?.Invoke();
        }
    }
}
