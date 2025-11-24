using Echobay.PlayerSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.MatchSystem
{
    public interface IMatchMaster
    {
        public event Action<MatchPlayer> OnPlayerCreated;

        public IReadOnlyList<MatchPlayer> Players { get; }

        public void Register(IMatchObserver observer);
        public void Unregister(IMatchObserver observer);
    }
}
