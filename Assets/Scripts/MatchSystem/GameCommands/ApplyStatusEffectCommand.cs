using Cysharp.Threading.Tasks;
using Echobay.PlayerSystem;
using Echobay.UnitSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.MatchSystem.TurnSystem
{
    public class ApplyStatusEffectCommand : GameCommand
    {
        private readonly MatchPlayer _player;
        private const float DelayBetweenUnits = 1f;

        public ApplyStatusEffectCommand(MatchPlayer matchPlayer, CancellationTokenObject tokenObject) : base(tokenObject)
        {
            _player = matchPlayer;
        }

        public override async UniTaskVoid OnTurnStarted()
        {
            foreach (Unit unit in _player.Units)
            {

            }
        }

        public override async UniTaskVoid OnTurnEnded()
        {
            foreach (Unit unit in _player.Units)
            {
                await UniTask.WaitForSeconds(DelayBetweenUnits, TokenObject);
            }
        }

        public override async UniTaskVoid OnTakeDamage()
        {
            foreach (Unit unit in _player.Units)
            {
                await UniTask.WaitForSeconds(DelayBetweenUnits, TokenObject);
            }
        }
    }
}
