using Echobay.GridSystem;
using UnityEngine;

namespace Echobay.ActionContext
{
    public class BlockActionMode : TargetSelectionMode
    {
        public override void Enter(ActionContextLinks contextLinks)
        {
            base.Enter(contextLinks);
            contextLinks.Grid.HideGrid();
            contextLinks.CardController.ClearCards();
        }

        public override void Exit()
        {
            base.Exit();
            ContextLinks.Grid.ShowGrid();
        }

        public override string GetDescription()
        {
            return string.Empty;
        }

        public override void HandleCellClick(GridCell cell)
        {
            
        }

        public override void OnCellEnter(GridCell cell)
        {

        }

        public override void OnCellExit(GridCell cell)
        {

        }

        public override void OnClickOnCard()
        {

        }
    }
}
