using Echobay.CardSystem;
using Echobay.GridSystem;
using Echobay.MatchSystem.TurnSystem;
using Echobay.PlayerSystem;
using System;
using UnityEngine;
using Zenject;

namespace Echobay.ActionContext
{
    public class ActionContextLinks : IDisposable
    {
        public MatchPlayer Player { get; private set; }
        public IGrid Grid { get; private set; }
        public IPathFinder PathFinder { get; private set; }
        public IInteractHandler InteractHandler { get; private set; }
        public ActionController ActionController { get; private set; }
        public GridPathView PathView { get; private set; }
        public ObjectInteractionViewData ViewData { get; private set; }
        public CardController CardController { get; private set; }

        private readonly ITurnInfo _turnInfo;

        [Inject]
        public ActionContextLinks(
            IGrid grid, 
            IPathFinder pathFinder, 
            IInteractHandler interactHandler,
            GridPathView pathView, 
            ObjectInteractionViewData objectInteractionView,
            ITurnInfo turnInfo,
            CardController cardController)
        {
            Grid = grid;
            PathFinder = pathFinder;
            InteractHandler = interactHandler;
            PathView = pathView;
            ViewData = objectInteractionView;
            CardController = cardController;

            _turnInfo = turnInfo;
        }

        public void Init(ActionController actionController)
        {
            ActionController = actionController;

            _turnInfo.OnTurnGained += OnTurnGained;
        }

        public void Dispose()
        {
            _turnInfo.OnTurnGained -= OnTurnGained;
        }

        private void OnTurnGained(MatchPlayer player)
        {
            Player = player;
        }
    }
}
