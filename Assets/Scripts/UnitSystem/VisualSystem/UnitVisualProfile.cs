using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.UnitSystem.VisualSystem
{
    [Serializable]
    public class UnitVisualProfile
    {
        [Title("Base")]
        public Gender Gender;

        [Title("Slots")]
        [ListDrawerSettings()]
        public List<VisualSlotRule> Slots;
    }

}
