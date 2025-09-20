using System;
using UnityEngine;

namespace Echobay.FightSystem.DamageType
{
    [Serializable]
    public class DamageSettings
    {
        public DamageTypeData DamageType;
        [Range(0, 2)] public float Resistance;
    }
}
