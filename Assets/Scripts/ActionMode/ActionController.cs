using Echobay.CardSystem;
using Echobay.GridSystem;
using Echobay.UnitSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Echobay.ActionContext
{
    public class ActionController : IInitializable, IDisposable
    {
        public event Action OnUnitSelected;
        public event Action OnActionExecuted;

        public TargetSelectionMode CurrentContext { get; private set; }
        public ICellOccupant SelectedUnit { get; private set; }
        public ICardAction SelectedAction { get; set; }
        public bool IsConfirmState { get; private set; }
        public Card SelectedCard { get; private set; }

        private readonly IPathFinder _pathFinder;
        private readonly SelectCellMode _selectCellContext = new();
        private readonly ActionContextLinks _contextLinks;
        private readonly CardController _cardController;

        [Inject]
        public ActionController(IPathFinder pathFinder, ActionContextLinks actionContextLinks, CardController cardController) 
        {
            _pathFinder = pathFinder;
            _contextLinks = actionContextLinks;
            _cardController = cardController;
        }

        public void Initialize()
        {
            _contextLinks.Init(this);

            _contextLinks.InteractHandler.OnPointEnter += OnCellEnter;
            _contextLinks.InteractHandler.OnPointExit += OnCellExit;
            _contextLinks.InteractHandler.OnInteract += OnCellInteracted;

            _cardController.OnClickOnCard += OnClickOnCard;
            _cardController.OnCardSelected += OnCardSelected;
            _cardController.OnCardDeselected += OnCardDeselected;

            SetContext(_selectCellContext);
        }

        public void Dispose()
        {
            if (SelectedAction != null)
            {
                SelectedAction.OnActionExecuted -= ActionExecuted;
            }

            _contextLinks.InteractHandler.OnPointEnter -= OnCellEnter;
            _contextLinks.InteractHandler.OnPointExit -= OnCellExit;
            _contextLinks.InteractHandler.OnInteract -= OnCellInteracted;

            _cardController.OnClickOnCard -= OnClickOnCard;
            _cardController.OnCardSelected -= OnCardSelected;
            _cardController.OnCardDeselected -= OnCardDeselected;
        }

        public void SelectUnit(ICellOccupant cellOccupant)
        {
            SelectedUnit = cellOccupant;
            OnUnitSelected?.Invoke();
        }

        public void SetContext(TargetSelectionMode actionContext)
        {
            if (CurrentContext != null)
            {
                CurrentContext.OnCompleted -= OnTargetSelectionCompleted;
                CurrentContext.Exit();
            }
            
            CurrentContext = actionContext;
            CurrentContext.OnCompleted += OnTargetSelectionCompleted;

            Debug.Log($"Context set to: {CurrentContext.GetType().Name}");

            CurrentContext.Enter(_contextLinks);
        }

        private void OnTargetSelectionCompleted(IReadOnlyCollection<GridCell> cells)
        {
            if (SelectedCard == null) return;

            //CardData cardData = SelectedCard.GetData<CardData>();
            ExecuteAction(SelectedAction, cells);
        }

        public void ResetContext()
        {
            IsConfirmState = false;

            _cardController.ClearCards();
            _contextLinks.Grid.ResetGrid();

            SetContext(_selectCellContext);
        }

        public void MoveAction(GridCell targetCell)
        {
            MoveAction moveAction = new(_pathFinder);

            SelectedAction = moveAction;
            SelectedAction.OnActionExecuted += ActionExecuted;

            ExecuteActionContext context = new(SelectedUnit, targetCell);
            moveAction.Execute(context);

            _cardController.ClearCards();
        }

        public void ExecuteAction(ICardAction cardAction, IReadOnlyCollection<GridCell> cells)
        {
            ExecuteActionContext context = new(SelectedUnit, cells);
            Card card = SelectedCard;

            if (cardAction.CanExecute(context))
            {
                cardAction.OnActionExecuted += ActionExecuted;
                cardAction.OnActionExecuted += SetEffects;

                cardAction.Execute(context);
            }

            _cardController.ClearCards();

            void SetEffects()
            {
                cardAction.OnActionExecuted -= SetEffects;

                CardData cardData = card.GetData<CardData>();
                ApplyEffects(cardData, context);
            }
        }

        private void ActionExecuted()
        {
            if (SelectedAction != null)
            {
                SelectedAction.OnActionExecuted -= ActionExecuted;
                SelectedAction = null;
            }

            ResetContext();

            OnActionExecuted?.Invoke();

            SelectUnit(null);
            SelectedCard = null;
        }

        private void ApplyEffects(CardData cardData, ExecuteActionContext context)
        {
            foreach (StatusEffectSettings effect in cardData.EffectsForAttacker)
            {
                Unit unit = (Unit)context.Executer;
                unit.AddEffect(effect.StatusEffect);
            }

            foreach (GridCell cell in context.TargetCells)
            {
                if (cell.Occupant is Unit unit)
                {
                    foreach (StatusEffectSettings effect in cardData.EffectsForTarget)
                    {
                        unit.AddEffect(effect.StatusEffect);
                    }
                }
            }
        }

        private void OnClickOnCard(Card card)
        {
            if (SelectedCard == card)
            {
                CurrentContext.OnClickOnCard();
            }
        }

        private void OnCardSelected(Card card)
        {
            if (SelectedCard == card || IsConfirmState) return;

            SelectedCard = card;

            CardData cardData = card.GetData<CardData>();
            SetContext(cardData.TargetingMode);

            IsConfirmState = true;

            SelectedAction = cardData.Action;
            _contextLinks.PathView.ClearPath();
        }

        private void OnCardDeselected(Card card)
        {
            IsConfirmState = false;
            SelectedCard = null;
            SetContext(new MoveTargetMode());

            SelectedAction.Exit();
        }

        private void OnCellEnter(IInteractable interactable)
        {
            if (interactable is GridCell cell)
            {
                CurrentContext?.OnCellEnter(cell);
            }
        }

        private void OnCellExit(IInteractable interactable)
        {
            if (interactable is GridCell cell)
            {
                CurrentContext?.OnCellExit(cell);
            }
        }

        private void OnCellInteracted(IInteractable interactable)
        {
            if (interactable is GridCell cell)
            {
                _cardController.OnCellSelected(interactable);
                CurrentContext?.HandleCellClick(cell);
            }
        }
    }
}
