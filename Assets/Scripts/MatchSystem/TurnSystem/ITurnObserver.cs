using UnityEngine;

namespace Echobay.MatchSystem.TurnSystem
{
    public interface ITurnObserver
    {
        public void OnRoundStarted();
        public void OnTurnEnded();
    }
}
