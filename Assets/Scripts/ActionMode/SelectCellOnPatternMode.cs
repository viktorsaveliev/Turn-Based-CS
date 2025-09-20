using Echobay.CardSystem;
using Echobay.GridSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.ActionContext
{
    public class SelectCellOnPatternMode : TargetSelectionMode
    {
        [SerializeField] private TargetAreaPattern _patternData;
        [SerializeField, Range(1, 5)] private int _targetCellCapacity = 1;

        public override void Enter(ActionContextLinks contextLinks)
        {
            if (_patternData == null)
            {
                Debug.LogError("Pattern data is null. Please ensure the card has a valid pattern.");
                return;
            }

            base.Enter(contextLinks);

            CellList.Clear();

            ContextLinks.Grid.SetActiveCells(false);

            ICellOccupant cellOccupant = ContextLinks.ActionController.SelectedUnit;

            List<GridCell> cells = ContextLinks.Grid.GetCellsByPattern(cellOccupant.CurrentCell, _patternData);

            foreach (GridCell cell in cells)
            {
                cell.SetColor(ContextLinks.ViewData.CellRegularColor);
                cell.SetActive(true);
            }
        }

        public override void Exit()
        {
            base.Exit();
            CellList.Clear();
        }

        public override string GetDescription()
        {
            return $"Select target cells ({CellList.Count}/{_targetCellCapacity})";
        }

        public override void HandleCellClick(GridCell cell)
        {
            if (true) //  && cell.IsOccupied
            {
                cell.SetColor(ContextLinks.ViewData.CellTargetedColor);
                CellList.Add(cell);

                Update();
            }

            if (CellList.Count >= _targetCellCapacity)
            {
                Complete();
            }
        }

        public override void OnCellEnter(GridCell cell)
        {
            if (cell.IsOccupied || CellList.Contains(cell))
            {
                cell.SetColor(ContextLinks.ViewData.CellTargetedColor);
            }
            else
            {
                cell.SetColor(ContextLinks.ViewData.CellSelectedColor);
            }
        }

        public override void OnCellExit(GridCell cell)
        {
            if (CellList.Contains(cell))
            {
                cell.SetColor(ContextLinks.ViewData.CellTargetedColor);
            }
            else
            {
                cell.SetColor(ContextLinks.ViewData.CellRegularColor);
            }
        }

        public override void OnClickOnCard()
        {

        }
    }
}
