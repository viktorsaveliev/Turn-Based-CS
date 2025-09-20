using Echobay.PlayerSystem;
using System;
using UnityEngine;

namespace Echobay.MatchSystem.TurnSystem
{
    public interface ITurnInfo
    {
        public event Action<Player> OnTurnGained;
        public event Action<Player> OnTurnLost;
    }
}
