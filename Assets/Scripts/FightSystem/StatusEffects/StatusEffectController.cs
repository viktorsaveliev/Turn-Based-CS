using Echobay.MatchSystem.TurnSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Echobay.FightSystem.StatusEffects
{
    public class StatusEffectController : ITurnObserver, IInitializable, IDisposable
    {
        private readonly HashSet<StatusEffectableObject> _units = new();
        private readonly ITurnMaster _master;

        [Inject]
        public StatusEffectController(ITurnMaster turnMaster)
        {
            _master = turnMaster;
        }

        public void Initialize()
        {
            _master.Register(this);
        }

        public void Dispose()
        {
            _master.Unregister(this);
        }

        public void OnRoundStarted()
        {
            foreach (StatusEffectableObject unit in _units)
            {
                unit.OnTurnStarted();
            }
        }

        public void OnTurnEnded()
        {
            foreach (StatusEffectableObject unit in _units)
            {
                unit.OnTurnEnded();
            }
        }
    }
}
