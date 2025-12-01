using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Echobay.MatchSystem.TurnSystem
{
    public abstract class GameCommand : IGameCommand
    {
        protected readonly CancellationTokenObject TokenObject;

        [Inject]
        public GameCommand(CancellationTokenObject tokenObject)
        {
            TokenObject = tokenObject;
        }

        public abstract UniTaskVoid OnTurnStarted();
        public abstract UniTaskVoid OnTurnEnded();
        public abstract UniTaskVoid OnTakeDamage();
    }
}
