using Echobay.PlayerSystem;
using System;
using UnityEngine;

namespace Echobay.MatchSystem
{
    public interface IMatchInfo
    {
        public Player LocalPlayer { get; }
    }
}
