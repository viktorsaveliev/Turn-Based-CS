using Echobay.PlayerSystem;
using System;
using UnityEngine;

namespace Echobay.MatchSystem.TurnSystem
{
    public interface ITurnInfo
    {
        public event Action<MatchPlayer> OnTurnGained;
        public event Action<MatchPlayer> OnTurnLost;
    }
}
