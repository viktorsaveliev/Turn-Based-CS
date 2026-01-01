using Echobay.UnitSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echobay
{
    [CreateAssetMenu(fileName = "Gameplay", menuName = "Game/Gameplay Data")]
    public class GameplayData : ScriptableObject
    {
        [field: Title("Base Settings")]
        [field: SerializeField] public UnitData[] DefaultUnitsForNewPlayer { get; private set; }

        [field: Title("Match Settings")]
        [field: SerializeField, Range(3, 15)] public int MatchStartDelayInSeconds { get; private set; } = 3;

        [field: Title("Turn Settings")]
        [field: SerializeField, Range(1, 5)] public int StandardTurnPointsPerRound { get; private set; } = 3;
        [field: SerializeField, Range(10, 120)] public int TimePerTurnInSeconds { get; private set; } = 60;
        [field: SerializeField, Range(0, 3)] public int SpendPointsForMovement { get; private set; } = 1;

    }
}
