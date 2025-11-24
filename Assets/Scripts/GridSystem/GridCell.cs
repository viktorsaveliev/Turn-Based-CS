using Echobay.InteractSystem;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Echobay.GridSystem
{
    public struct CellNeighborData
    {
        public GridCell Cell;
        public int Cost;
    }

    public class GridCell : InteractableObject
    {
        public bool IsOccupied => Occupant != null;

        [field: SerializeField] public Vector2Int Position { get; set; }

        [field: ShowInInspector] public ICellOccupant Occupant { get; private set; }
        public CellNeighborData[] Neighbors { get; set; }

        [SerializeField] private MeshRenderer _cellRenderer;

        private ObjectInteractionViewData _interactionView;

        private void OnValidate()
        {
            if (_cellRenderer == null)
            {
                _cellRenderer = GetComponent<MeshRenderer>();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.Label(transform.position, Position.ToString());
        }
#endif

        [Inject]
        public void Construct(ObjectInteractionViewData interactionView)
        {
            _interactionView = interactionView;
        }

        public void SetColor(Color color)
        {
            _cellRenderer.material.color = color;
        }

        public override void OnPointEnter()
        {

        }

        public override void OnPointExit()
        {

        }

        public void SetActive(bool isActive)
        {
            IsInteractable = isActive;
            _cellRenderer.material.color = IsInteractable ? _interactionView.CellRegularColor : _interactionView.CellUnavailableColor;
        }

        public void SetOccupant(ICellOccupant occupant)
        {
            if (occupant == null)
            {
                Occupant = null;
                return;
            }

            if (IsOccupied)
            {
                Debug.LogError("Cell is already occupied. Cannot set a new occupant.");
                return;
            }

            Occupant = occupant;
            occupant.CurrentCell = this;
        }
    }
}
