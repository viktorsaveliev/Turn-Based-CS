using Echobay.PlayerSystem;
using System;
using UnityEngine;

namespace Echobay.MatchSystem
{
    public interface IMatchMaster
    {
        public event Action<Player> OnPlayerCreated;

        public void Register(IMatchObserver observer);
        public void Unregister(IMatchObserver observer);
    }
}
