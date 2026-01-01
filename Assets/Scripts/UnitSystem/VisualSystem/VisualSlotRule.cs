using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Echobay.UnitSystem.VisualSystem
{
    [Serializable]
    public class VisualSlotRule
    {
        [Title("Visual Slot")]

        public VisualSlot Slot;

        public bool Enabled = true;
        public bool Randomize = true;

        [HideIf(nameof(Randomize), true)]
        public VisualElementData FixedElement;
    }

}
