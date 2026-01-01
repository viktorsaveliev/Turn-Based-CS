using Echobay.CardSystem;
using Echobay.FightSystem.DamageType;
using Echobay.FightSystem.Reaction;
using Echobay.UnitSystem.VisualSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Echobay.UnitSystem
{
    [CreateAssetMenu(fileName = "(UnitData) ", menuName = "Units/BaseUnitData")]
    public class UnitData : ScriptableObject
    {
        [field: Title("Base")]
        [field: SerializeField, PreviewField(ObjectFieldAlignment.Left, Height = 80)] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField, Range(0, 10)] public int MoveMaxDistance { get; private set; } = 3;
        [field: SerializeField, Range(0, 10)] public int MoveSpeed { get; private set; } = 3;
        [field: SerializeField, Range(1, 1000)] public int MaxHealth { get; private set; } = 100;
        [field: SerializeField] public Unit Prefab { get; private set; }
        [field: SerializeField] public AnimationCommand[] MoveAnimation { get; private set; }

        public IReadOnlyCollection<CardData> CardsList => _cardsList;

        [SerializeField] private CardData[] _cardsList;

        [field: Title("Behaviour")]
        [field: SerializeField] public DamageModifiers DamageModifiers { get; private set; }
        [field: SerializeReference] public IReaction[] OnTakeDamageReaction { get; private set; }
        [field: SerializeReference] public IReaction[] OnDeathReaction { get; private set; }

        [field: Title("Visuals")]
        [field: SerializeField] public UnitVisualProfile VisualProfile { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EnsureVisualProfile();
        }
#endif

        private void EnsureVisualProfile()
        {
            if (VisualProfile == null)
            {
                VisualProfile = new UnitVisualProfile();
            }

            if (VisualProfile.Slots == null)
            {
                VisualProfile.Slots = new List<VisualSlotRule>();
            }

            foreach (VisualSlot slot in Enum.GetValues(typeof(VisualSlot)))
            {
                if (VisualProfile.Slots.Any(s => s.Slot == slot))
                    continue;

                VisualProfile.Slots.Add(new VisualSlotRule
                {
                    Enabled = true,
                    Slot = slot,
                    Randomize = true,
                    FixedElement = null
                });
            }
        }
    }
}