using Echobay.ActionContext;
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
        private ActionController _controller;

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

        private void OnDestroy()
        {
            _controller.OnUnitSelected -= OnUnitSelected;
        }

        public void Init(ActionController controller)
        {
            _controller = controller;
            _controller.OnUnitSelected += OnUnitSelected;
        }

        public void ClearCards()
        {
            foreach (Card card in _cardPool)
            {
                card.OnClicked -= OnClickedOnCard;
                Destroy(card.gameObject);
            }

            SelectedCard = null;
            _cardPool.Clear();
        }

        private void OnUnitSelected()
        {
            if (_controller.SelectedUnit == null)
            {
                ClearCards();
            }
            else
            {
                Unit unit = (Unit)_controller.SelectedUnit;
                CreateCards(unit);
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
