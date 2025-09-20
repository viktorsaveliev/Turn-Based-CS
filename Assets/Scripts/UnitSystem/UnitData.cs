using Echobay.CardSystem;
using Echobay.FightSystem.DamageType;
using Echobay.FightSystem.Reaction;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.UnitSystem
{
    [CreateAssetMenu(fileName = "(UnitData) ", menuName = "Units/BaseUnitData")]
    public class UnitData : ScriptableObject
    {
        [field: Title("Base")]
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField, Range(0, 10)] public int MoveMaxDistance { get; private set; } = 3;
        [field: SerializeField, Range(0, 10)] public int MoveSpeed { get; private set; } = 3;
        [field: SerializeField, Range(1, 1000)] public int MaxHealth { get; private set; } = 100;
        [field: SerializeField] public Unit Prefab { get; private set; }

        public IReadOnlyCollection<CardData> CardsList => _cardsList;

        [SerializeField] private CardData[] _cardsList;

        [field: Title("Behaviour")]
        [field: SerializeField] public DamageSettings[] Immunity { get; private set; }
        [field: SerializeField] public DamageSettings[] Vulnerability { get; private set; }
        [field: SerializeReference] public IReaction[] OnTakeDamageReaction { get; private set; }
        [field: SerializeReference] public IReaction[] OnDeathReaction { get; private set; }
    }
}