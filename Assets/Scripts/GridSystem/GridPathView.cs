using Echobay.ActionContext;
using Echobay.PlayerSystem;
using IndieMarc.CurvedLine;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Echobay.GridSystem
{
    public class GridPathView : MonoBehaviour
    {
        [SerializeField] private CurvedLine2D _curvedLine2D;
        [SerializeField] private GridManager _manager;

        private ActionController _actionController;

        [Inject]
        public void Construct(ActionController actionController)
        {
            _actionController = actionController;
        }

        private void OnValidate()
        {
            if (_curvedLine2D == null)
            {
                _curvedLine2D = GetComponent<CurvedLine2D>();
            }
        }

        public void ShowPath(GridCell targetCell)
        {
            if (_actionController.SelectedUnit == null)
            {
                _curvedLine2D.paths = null;
                _curvedLine2D.Hide();
                return;
            }

            List<GridCell> path = _manager.FindPath(_actionController.SelectedUnit.CurrentCell, targetCell);
            if (path != null && path.Count > 0)
            {
                _curvedLine2D.paths = new Transform[path.Count];

                for (int i = 0; i < path.Count; i++)
                {
                    _curvedLine2D.paths[i] = path[i].transform;
                }

                _curvedLine2D.Show();
            }
            else
            {
                _curvedLine2D.paths = null;
                _curvedLine2D.Hide();
            }
        }

        public void ClearPath()
        {
            _curvedLine2D.paths = null;
            _curvedLine2D.Hide();
        }
    }
}
