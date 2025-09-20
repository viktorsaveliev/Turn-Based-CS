using Echobay.GridSystem;
using Echobay.InputSystem;
using Echobay.UnitSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Echobay.CardSystem
{
    public class CardController : MonoBehaviour
    {
        public event Action<Card> OnClickOnCard;
        public event Action<Card> OnCardSelected;
        public event Action<Card> OnCardDeselected;
        public event Action<Card> OnCardPlayed;

        public Card SelectedCard { get; private set; }

        [SerializeField] private Transform _cardContainer;

        private readonly List<Card> _cardPool = new();
        private InputData _inputData;

        [Inject]
        public void Construct(InputData inputData)
        {
            _inputData = inputData;
        }

        private void OnEnable()
        {
            _inputData.OnCanceled += DeselectCard;
        }

        private void OnDisable()
        {
            _inputData.OnCanceled -= DeselectCard;

            DeselectCard();
            ClearCards();
        }

        public void ClearCards()
        {
            foreach (Card card in _cardPool)
            {
                card.OnClicked -= OnClickedOnCard;
                Destroy(card.gameObject);
            }

            //DeselectCard();
            SelectedCard = null;
            _cardPool.Clear();
        }

        public void OnCellSelected(IInteractable interactable)
        {
            GridCell gridCell = interactable as GridCell;

            if (SelectedCard == null)
            {
                if (gridCell != null && gridCell.IsOccupied && gridCell.Occupant is Unit unit)
                {
                    CreateCards(unit);
                }
            }
        }

        private void CreateCards(Unit unit)
        {
            ClearCards();

            foreach (CardData cardData in unit.GetData<UnitData>().CardsList)
            {
                Card card = Instantiate(cardData.Prefab, _cardContainer);
                card.Init(cardData);
                card.OnClicked += OnClickedOnCard;

                _cardPool.Add(card);
            }
        }

        private void OnClickedOnCard(Card card)
        {
            OnClickOnCard?.Invoke(card);

            if (SelectedCard == card)
            {
                return;
            }

            SelectCard(card);
        }

        private void SelectCard(Card card)
        {
            if (SelectedCard != null)
            {
                DeselectCard();
            }

            SelectedCard = card;
            OnCardSelected?.Invoke(card);
        }

        private void DeselectCard()
        {
            if (SelectedCard != null)
            {
                OnCardDeselected?.Invoke(SelectedCard);
                SelectedCard = null;
            }
        }
    }
}
