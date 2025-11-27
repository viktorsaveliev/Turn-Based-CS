using UnityEngine;

namespace Echobay.MatchSystem.TurnSystem
{
    public interface ITurnObserver
    {
        public void OnTurnStarted();
        public void OnTurnEnded();
    }
}
