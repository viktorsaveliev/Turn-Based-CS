using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Echobay.MatchSystem.TurnSystem
{
    public interface IGameCommand
    {
        public UniTaskVoid OnTurnStarted();
        public UniTaskVoid OnTurnEnded();
        public UniTaskVoid OnTakeDamage();
    }
}
