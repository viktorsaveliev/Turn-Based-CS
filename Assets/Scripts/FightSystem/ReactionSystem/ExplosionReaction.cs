using System;
using UnityEngine;

namespace Echobay.FightSystem.Reaction
{
    public class ExplosionReaction : Reaction
    {
        [field: SerializeField, Range(1, 10)] public float Radius { get; private set; } = 1f;
        
        private IExplodable _explodable;

        /*public ExplosionReaction(IExplodable explodableObject)
        {
            _explodable = explodableObject;
        }*/

        public override void Execute()
        {
            _explodable.Explode(Radius);
        }
    }
}
