using UnityEngine;

namespace Echobay.MatchSystem.TurnSystem
{
    public interface ITurnMaster
    {
        public int CurrentRound { get; }
        public int TimeRemaining { get; }
        public void Register(ITurnObserver observer);
        public void Unregister(ITurnObserver observer);
    }
}
