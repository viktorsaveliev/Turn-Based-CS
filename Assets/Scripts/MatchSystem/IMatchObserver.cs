using UnityEngine;

namespace Echobay.MatchSystem
{
    public interface IMatchObserver
    {
        public void OnMatchStarted();
        public void OnMatchEnded();
    }
}
