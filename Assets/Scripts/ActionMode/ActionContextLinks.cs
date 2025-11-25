using Echobay.CardSystem;
using Echobay.GridSystem;
using Echobay.PlayerSystem;
using UnityEngine;
using Zenject;

namespace Echobay.ActionContext
{
    public class ActionContextLinks
    {
        public MatchPlayer Player { get; private set; }
        public IGrid Grid { get; private set; }
        public IPathFinder PathFinder { get; private set; }
        public IInteractHandler InteractHandler { get; private set; }
        public ActionController ActionController { get; private set; }
        public GridPathView PathView { get; private set; }
        public ObjectInteractionViewData ViewData { get; private set; }
        public CardController CardController { get; private set; }

        [Inject]
        public ActionContextLinks(
            IGrid grid, 
            IPathFinder pathFinder, 
            IInteractHandler interactHandler,
            GridPathView pathView, 
            ObjectInteractionViewData objectInteractionView,
            CardController cardController)
        {
            Grid = grid;
            PathFinder = pathFinder;
            InteractHandler = interactHandler;
            PathView = pathView;
            ViewData = objectInteractionView;
            CardController = cardController;
        }

        public void Init(ActionController actionController)
        {
            ActionController = actionController;
        }

        public void SetCurrentPlayer(MatchPlayer player)
        {
            Player = player;
        }
    }
}
