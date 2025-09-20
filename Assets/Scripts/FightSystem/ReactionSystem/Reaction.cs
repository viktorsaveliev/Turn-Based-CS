using System;
using UnityEngine;

namespace Echobay.FightSystem.Reaction
{
    [Serializable]
    public abstract class Reaction : IReaction
    {
        public abstract void Execute();
    }
}
