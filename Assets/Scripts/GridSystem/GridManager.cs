using Echobay.CardSystem;
using Echobay.UnitSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Echobay.GridSystem
{
    [Serializable]
    public class CellOccupantSetting
    {
        public GridCell TargetCell;
        public Unit Unit;
    }

    public class GridManager : MonoBehaviour, IGrid, IPathFinder
    {
        public IReadOnlyCollection<GridCell> GridCells => _gridCellEditors;

        [SerializeField] private GridCell _cellPrefab;
        [SerializeField] private Vector2Int _gridSize = new(10, 10);

        [SerializeField] private List<GridCell> _gridCellEditors = new();
        [SerializeField] private CellOccupantSetting[] _cellOccupants;

        private void Awake()
        {
            InitializeNeighbors();

            foreach (CellOccupantSetting cellOccupantSetting in _cellOccupants)
            {
                cellOccupantSetting.TargetCell.SetOccupant(cellOccupantSetting.Unit);
                cellOccupantSetting.Unit.transform.position = cellOccupantSetting.TargetCell.transform.position + new Vector3(0, 0.25f, 0);
            }
        }

        public List<GridCell> GetCellsByPattern(GridCell origin, TargetAreaPattern pattern)
        {
            return GetCellsByOffsets(origin, pattern.AffectedCells);
        }

        public List<GridCell> GetCellsByOffsets(GridCell origin, IReadOnlyCollection<Vector2Int> offsets)
        {
            List<GridCell> result = new();

            foreach (Vector2Int offset in offsets)
            {
                Vector2Int pos = origin.Position + offset;
                GridCell cell = GetCellByPosition(pos);

                if (cell != null)
                {
                    result.Add(cell);
                }
            }

            return result;
        }

        public void ShowCellsInRadius(GridCell centerCell, int cost)
        {
            List<GridCell> cells = GetCellsInRadius(centerCell, cost);
            //ShowCells(cells);

            foreach (GridCell cell in cells)
            {
                cell.SetActive(true);
            }
        }

        public void ResetGrid()
        {
            SetActiveCells(true);
        }

        private void InitializeNeighbors()
        {
            Vector2Int[] Directions =
            {
                new Vector2Int(0, 1),   // вверх
                new Vector2Int(1, 0),   // вправо
                new Vector2Int(0, -1),  // вниз
                new Vector2Int(-1, 0),  // влево
                new Vector2Int(1, 1),   // вверх-вправо (диагональ)
                new Vector2Int(1, -1),  // вниз-вправо
                new Vector2Int(-1, -1), // вниз-влево
                new Vector2Int(-1, 1),  // вверх-влево
            };

            foreach (var cell in _gridCellEditors) // или что у тебя там
            {
                List<CellNeighborData> neighbors = new();

                foreach (var dir in Directions)
                {
                    Vector2Int neighborPos = cell.Position + dir;
                    var neighbor = GetCellByPosition(neighborPos);
                    if (neighbor == null)
                        continue;

                    int cost = (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) == 2) ? 2 : 1; // диагональ = 2, иначе = 1

                    neighbors.Add(new CellNeighborData
                    {
                        Cell = neighbor,
                        Cost = cost
                    });
                }

                cell.Neighbors = neighbors.ToArray();
            }
        }

        public void SetActiveCells(bool isActive)
        {
            foreach (var cell in _gridCellEditors)
            {
                cell.SetActive(isActive);
            }
        }

        public void ShowGrid()
        {
            foreach (var cell in _gridCellEditors)
            {
                if (cell != null)
                {
                    cell.gameObject.SetActive(true);
                }
            }
        }

        public void ShowCells(List<GridCell> cells)
        {
            foreach (var cell in cells)
            {
                if (cell != null)
                {
                    cell.gameObject.SetActive(true);
                }
            }
        }

        public void HideGrid()
        {
            foreach (var cell in _gridCellEditors)
            {
                if (cell != null)
                {
                    cell.gameObject.SetActive(false);
                }
            }
        }

        public List<GridCell> GetCellsInRadius(GridCell centerCell, int maxSteps)
        {
            var result = new List<GridCell>();
            var visited = new HashSet<GridCell>();
            var queue = new Queue<(GridCell cell, int costSoFar)>();

            queue.Enqueue((centerCell, 0));
            visited.Add(centerCell);

            while (queue.Count > 0)
            {
                var (currentCell, costSoFar) = queue.Dequeue();

                result.Add(currentCell);

                foreach (var neighborData in currentCell.Neighbors)
                {
                    var neighbor = neighborData.Cell;
                    int stepCost = neighborData.Cost;

                    if (neighbor == null || visited.Contains(neighbor))
                        continue;

                    if (neighbor.IsOccupied) // neighbor.IsInteractable || 
                        continue;

                    int newCost = costSoFar + stepCost;
                    if (newCost > maxSteps)
                        continue;

                    visited.Add(neighbor);
                    queue.Enqueue((neighbor, newCost));
                }
            }

            return result;
        }

        [Button("Generate Grid")]
        public void GenerateGrid()
        {
            Vector3 offset = new(
                (_gridSize.x - 1) / 2f,
                0,
                (_gridSize.y - 1) / 2f
            );

            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    Vector3 position = new Vector3(x, 0, y) - offset;
                    GridCell cell = Instantiate(_cellPrefab, position, Quaternion.identity, transform);
                    _gridCellEditors.Add(cell);
                    cell.Position = new Vector2Int(x, y);
                }
            }
        }

        [Button("Destroy Grid")]
        public void DestroyGrid()
        {
            foreach (var cell in _gridCellEditors)
            {
                if (cell != null)
                {
                    DestroyImmediate(cell.gameObject);
                }
            }

            _gridCellEditors.Clear();
        }

        public List<GridCell> GetReachableCells(Vector2Int from, int maxCost)
        {
            List<GridCell> reachable = new();
            Queue<(Vector2Int pos, int cost)> frontier = new();
            HashSet<Vector2Int> visited = new();

            frontier.Enqueue((from, 0));
            visited.Add(from);

            while (frontier.Count > 0)
            {
                var (currentPos, currentCost) = frontier.Dequeue();
                GridCell currentCell = GetCellByPosition(currentPos);
                reachable.Add(currentCell);

                foreach (Vector2Int neighborPos in GetNeighborPositions(currentPos))
                {
                    if (visited.Contains(neighborPos)) continue;

                    GridCell neighbor = GetCellByPosition(neighborPos);
                    if (!neighbor.IsInteractable || neighbor.IsOccupied) continue;

                    int newCost = currentCost + 1; // или использовать neighbor.MoveCost если есть
                    if (newCost <= maxCost)
                    {
                        visited.Add(neighborPos);
                        frontier.Enqueue((neighborPos, newCost));
                    }
                }
            }

            return reachable;
        }

        public List<GridCell> FindPath(GridCell start, GridCell target)
        {
            PathNode startNode = new(start);
            PathNode targetNode = new(target);

            List<PathNode> openSet = new() { startNode };
            HashSet<Vector2Int> closedSet = new();
            Dictionary<Vector2Int, PathNode> allNodes = new() { [start.Position] = startNode };

            while (openSet.Count > 0)
            {
                PathNode current = openSet.OrderBy(n => n.FCost).ThenBy(n => n.HCost).First();
                openSet.Remove(current);
                closedSet.Add(current.Cell.Position);

                if (current.Cell.Position == target.Position)
                {
                    return ReconstructPath(current);
                }

                foreach (Vector2Int neighborPos in GetNeighborPositions(current.Cell.Position))
                {
                    if (closedSet.Contains(neighborPos)) continue;

                    if (!allNodes.TryGetValue(neighborPos, out var neighbor))
                    {
                        GridCell cell = GetCellByPosition(neighborPos);
                        if (cell.IsOccupied) continue; // !cell.IsInteractable || 

                        neighbor = new PathNode(cell);
                        allNodes[neighborPos] = neighbor;
                    }

                    // Определяем, диагональное ли направление
                    bool isDiagonal = current.Cell.Position.x != neighbor.Cell.Position.x && current.Cell.Position.y != neighbor.Cell.Position.y;
                    int moveCost = isDiagonal ? 14 : 10;

                    int tentativeG = current.GCost + moveCost;

                    if (tentativeG < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = tentativeG;

                        int dx = Mathf.Abs(neighbor.Cell.Position.x - target.Position.x);
                        int dy = Mathf.Abs(neighbor.Cell.Position.y - target.Position.y);
                        neighbor.HCost = 10 * (dx + dy) + (14 - 2 * 10) * Mathf.Min(dx, dy); // диагональная эвристика

                        neighbor.Parent = current;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }

        private bool IsInBounds(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < _gridSize.x && pos.y >= 0 && pos.y < _gridSize.y;
        }

        private List<Vector2Int> GetNeighborPositions(Vector2Int pos)
        {
            List<Vector2Int> neighbors = new();

            Vector2Int[] offsets =
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right,
                new(1, 1),
                new(1, -1),
                new(-1, -1),
                new(-1, 1)
            };

            foreach (var offset in offsets)
            {
                var neighbor = pos + offset;
                if (!IsInBounds(neighbor)) continue;

                if (Mathf.Abs(offset.x) + Mathf.Abs(offset.y) == 2)
                {
                    var cell1 = GetCellByPosition(new Vector2Int(pos.x + offset.x, pos.y));
                    var cell2 = GetCellByPosition(new Vector2Int(pos.x, pos.y + offset.y));
                    if ((cell1 != null && (cell1.IsOccupied)) ||
                        (cell2 != null && (cell2.IsOccupied)))
                        continue;
                }

                neighbors.Add(neighbor);
            }

            return neighbors;
        }

        private GridCell GetCellByPosition(Vector2Int pos)
        {
            if (IsInBounds(pos))
            {
                foreach (var cell in _gridCellEditors)
                {
                    if (cell.Position == pos)
                    {
                        return cell;
                    }
                }
            }

            return null;
        }

        private List<GridCell> ReconstructPath(PathNode endNode)
        {
            List<GridCell> path = new();

            var current = endNode;
            while (current != null)
            {
                path.Add(current.Cell);
                current = current.Parent;
            }

            path.Reverse();
            return path;
        }
    }
}