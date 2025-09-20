using UnityEngine;

namespace Echobay.MatchSystem
{
    public interface IMatchMode
    {
        public void SetupPlayers(MatchController matchController);
    }
}