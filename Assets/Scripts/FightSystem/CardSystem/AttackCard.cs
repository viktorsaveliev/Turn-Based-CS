using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Echobay.CardSystem
{
    public class AttackCard : Card
    {
        [Title("Attack Info")]
        [SerializeField] private TMP_Text _damageText;

        public override void Init(CardData cardData)
        {
            base.Init(cardData);

            AttackCardData attackData = (AttackCardData)Data;
            _damageText.text = $"DMG: <color=red>{attackData.Action.Value}</color>";
        }
    }
}
