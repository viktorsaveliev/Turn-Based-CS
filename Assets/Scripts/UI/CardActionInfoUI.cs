using Echobay.ActionContext;
using Echobay.CardSystem;
using Echobay.GridSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Echobay.UISystem
{
    public class CardActionInfoUI : PanelUI
    {
        [SerializeField] private TMP_Text _description;

        private CardController _cardController;
        private TargetSelectionMode _selectionMode;

        [Inject]
        public void Construct(CardController cardController)
        {
            _cardController = cardController;
        }

        private void OnEnable()
        {
            _cardController.OnCardSelected += ShowDescription;
            _cardController.OnCardDeselected += HideText;
        }

        private void OnDisable()
        {
            _cardController.OnCardSelected -= ShowDescription;
            _cardController.OnCardDeselected -= HideText;

            HideText(null);
        }

        private void ShowDescription(Card card) 
        {
            _selectionMode = card.GetData<CardData>().TargetingMode;

            string description = _selectionMode.GetDescription();
            _description.text = description;

            _selectionMode.OnCompleted += OnCompleteTurn;
            _selectionMode.OnUpdated += UpdateDescription;

            _description.gameObject.SetActive(true);
        }

        private void UpdateDescription()
        {
            string description = _selectionMode.GetDescription();
            _description.text = description;
        }

        private void HideText(Card card)
        {
            _selectionMode.OnCompleted -= OnCompleteTurn;
            _selectionMode.OnUpdated -= UpdateDescription;
            _selectionMode = null;

            _description.gameObject.SetActive(false);
        }

        private void OnCompleteTurn(IReadOnlyCollection<GridCell> cells)
        {
            HideText(null);
        }
    }
}
