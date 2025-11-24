using Echobay.CardSystem;
using Echobay.GridSystem;
using Echobay.UnitSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Echobay.ActionContext
{
    public class ActionController : IDisposable
    {
        public event Action OnUnitSelected;
        public event Action OnActionExecuted;
        public event Action<TargetSelectionMode> OnSelectionModeChanged;

        public event Action<IUnitCellOccupant, GridCell> OnMoveRequested;
        public event Action<CardData, ExecuteActionContext> OnActionRequested;

        public TargetSelectionMode CurrentSelectionMode { get; private set; }
        public IUnitCellOccupant SelectedUnit { get; private set; }
        public ICardAction SelectedAction { get; set; }
        public bool IsConfirmState { get; private set; }
        public bool IsActionExecuting { get; private set; }
        public Card SelectedCard { get; private set; }

        private readonly SelectCellMode _selectCellMode = new();
        private readonly ActionContextLinks _contextLinks;
        private readonly CardController _cardController;

        [Inject]
        public ActionController(ActionContextLinks actionContextLinks, CardController cardController) 
        {
            _contextLinks = actionContextLinks;
            _cardController = cardController;
        }

        public void Init()
        {
            _contextLinks.Init(this);

            _contextLinks.InteractHandler.OnPointEnter += OnCellEnter;
            _contextLinks.InteractHandler.OnPointExit += OnCellExit;
            _contextLinks.InteractHandler.OnInteract += OnCellInteracted;

            _cardController.OnClickOnCard += OnClickOnCard;
            _cardController.OnCardSelected += OnCardSelected;
            _cardController.OnCardDeselected += OnCardDeselected;

            BlockActions();
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

        public void SelectUnit(IUnitCellOccupant cellOccupant)
        {
            SelectedUnit = cellOccupant;
            OnUnitSelected?.Invoke();
        }

        public void SetSelectionMode(TargetSelectionMode actionContext)
        {
            if (CurrentSelectionMode != null)
            {
                CurrentSelectionMode.OnCompleted -= OnTargetSelectionCompleted;
                CurrentSelectionMode.Exit();
            }
            
            CurrentSelectionMode = actionContext;
            CurrentSelectionMode.OnCompleted += OnTargetSelectionCompleted;

            Debug.Log($"Context set to: {CurrentSelectionMode.GetType().Name}");

            CurrentSelectionMode.Enter(_contextLinks);

            OnSelectionModeChanged?.Invoke(CurrentSelectionMode);
        }

        public void SelecttCellAction() => SetSelectionMode(_selectCellMode); 
        public void BlockActions() => SetSelectionMode(new BlockActionMode());

        private void OnTargetSelectionCompleted(IReadOnlyCollection<GridCell> cells)
        {
            if (SelectedCard == null) return;

            ExecuteActionContext context = new(SelectedAction, SelectedUnit, cells);
            RequestAction(SelectedCard.Data, context);
            BlockActions();
        }

        public void ResetContext()
        {
            IsConfirmState = false;

            _cardController.ClearCards();
            _contextLinks.Grid.ResetGrid();

            SetSelectionMode(_selectCellMode);
        }

        public void RequestMove(GridCell target)
        {
            if (SelectedUnit == null) return;

            OnMoveRequested?.Invoke(SelectedUnit, target);
        }

        public void RequestAction(CardData cardData, ExecuteActionContext context)
        {
            if (IsActionExecuting)
            {
                Debug.LogError("[RequestAction]: Action already active");
                return;
            }

            if (SelectedUnit == null || SelectedAction == null)
            {
                Debug.LogError("Unit or action null reference");
                return;
            }

            IsActionExecuting = true;
            OnActionRequested?.Invoke(cardData, context);
        }

        public void ActionExecuted()
        {
            CancelAction();

            OnActionExecuted?.Invoke();
        }

        public void ActionExecuted(ExecuteActionContext context)
        {
            ApplyEffects(SelectedCard.Data, context);
            ActionExecuted();
        }

        public void CancelAction()
        {
            if (SelectedAction != null)
            {
                SelectedAction.OnActionExecuted -= ActionExecuted;
                SelectedAction = null;
            }

            ResetContext();

            SelectUnit(null);
            SelectedCard = null;

            IsActionExecuting = false;
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
                CurrentSelectionMode.OnClickOnCard();
            }
        }

        private void OnCardSelected(Card card)
        {
            if (SelectedCard == card || IsConfirmState) return;

            SelectedCard = card;

            CardData cardData = card.Data;
            SetSelectionMode(cardData.TargetingMode);

            IsConfirmState = true;

            SelectedAction = cardData.Action;
            _contextLinks.PathView.ClearPath();
        }

        private void OnCardDeselected(Card card)
        {
            IsConfirmState = false;
            SelectedCard = null;
            SetSelectionMode(new MoveTargetMode());

            SelectedAction.Exit();
        }

        private void OnCellEnter(IInteractable interactable)
        {
            if (interactable is GridCell cell)
            {
                CurrentSelectionMode?.OnCellEnter(cell);
            }
        }

        private void OnCellExit(IInteractable interactable)
        {
            if (interactable is GridCell cell)
            {
                CurrentSelectionMode?.OnCellExit(cell);
            }
        }

        private void OnCellInteracted(IInteractable interactable)
        {
            if (interactable is GridCell cell)
            {
                _cardController.OnCellSelected(interactable);
                CurrentSelectionMode?.HandleCellClick(cell);
            }
        }
    }
}
