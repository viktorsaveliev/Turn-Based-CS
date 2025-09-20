using System;
using UnityEngine;

namespace Echobay.CardSystem
{
    public interface ICard
    {
        public event Action<Card> OnClicked;

        public void Init(CardData cardData);
        public void OnClick();
    }
}
