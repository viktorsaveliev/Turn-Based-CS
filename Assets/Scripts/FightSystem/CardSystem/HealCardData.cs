using Sirenix.OdinInspector;
using UnityEngine;

namespace Echobay.CardSystem
{
    [CreateAssetMenu(menuName = "Cards/HealCardData")]
    public class HealCardData : CardData
    {
        public int Heal => _heal;

        [Title("Heal Data")]
        [SerializeField, Range(1, 50)] private int _heal = 1;
    }
}
