using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Echobay.FightSystem.DamageType
{
    [Serializable]
    public class DamageModifier
    {
        public DamageTypeData DamageType;

        [InfoBox("Damage modification factor:\n" +
             "0 = complete immunity\n" +
             "1 = normal damage\n" +
             ">1 = vulnerability (increased damage)",
             InfoMessageType.Info)]
        [Range(0, 2)] public float Resistance = 1;
    }

    [Serializable]
    public class DamageModifiers
    {
        public DamageModifier[] Modifiers;
    }
}
