using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.CardSystem
{
    [CreateAssetMenu(menuName = "Combat/TargetAreaPattern")]
    public class TargetAreaPattern : SerializedScriptableObject
    {
        [field: SerializeField] public string PatternName { get; private set; }

        [SerializeField, TableMatrix(HorizontalTitle = "Grid Matrix", SquareCells = true,
             DrawElementMethod = nameof(DrawCell),
             ResizableColumns = false)]
        private bool[,] _gridMatrix = new bool[11, 11];

        public IReadOnlyCollection<Vector2Int> AffectedCells
        {
            get
            {
                return GenerateAffectedCellsFromMatrix();
            }
        }

        private List<Vector2Int> GenerateAffectedCellsFromMatrix()
        {
            var result = new List<Vector2Int>();
            int size = _gridMatrix.GetLength(0);
            int center = size / 2;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (_gridMatrix[x, y])
                    {
                        result.Add(new Vector2Int(x - center, y - center));
                    }
                }
            }

            return result;
        }

        private static bool DrawCell(Rect rect, bool value, int x, int y)
        {
            bool isCenter = (x == 5 && y == 5);

            Color originalColor = GUI.backgroundColor;

            if (isCenter)
            {
                GUI.backgroundColor = Color.green;
                GUI.Box(rect, "Center");
                GUI.backgroundColor = originalColor;
                return true;
            }

            if (GUI.Button(rect, value ? "X" : ""))
            {
                value = !value;
            }

            return value;
        }
    }
}