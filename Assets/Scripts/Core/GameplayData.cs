using Sirenix.OdinInspector;
using UnityEngine;

namespace Echobay
{
    [CreateAssetMenu(fileName = "Gameplay", menuName = "Game/Gameplay Data")]
    public class GameplayData : ScriptableObject
    {
        [field: Title("Match Settings")]
        [field: SerializeField, Range(3, 15)] public int MatchStartDelayInSeconds { get; private set; } = 3;

        [field: Title("Turn Settings")]
        [field: SerializeField, Range(1, 5)] public int StandardTurnPointsPerRound { get; private set; } = 3;
        [field: SerializeField, Range(30, 120)] public int TimePerTurnInSeconds { get; private set; } = 60;

    }
}
