using UnityEngine;

[CreateAssetMenu(fileName = "VisualDataConfig", menuName = "Game/Visual Data Config")]
public class ObjectInteractionViewData : ScriptableObject
{
    public float OutlineSizeOnEnter => _outlineSizeOnEnter;
    public float OutlineSizeOnSelect => _outlineSizeOnSelect;

    [field: Header("Outline Settings")]
    [field: SerializeField] public Color PointEnterOutlineColor { get; private set; }
    [field: SerializeField] public Color SelectedOutlineColor { get; private set; }

    [SerializeField, Range(0, 5)] private float _outlineSizeOnEnter;
    [SerializeField, Range(0, 5)] private float _outlineSizeOnSelect;

    [field: Header("Cell Colors")]
    [field: SerializeField] public Color CellSelectedColor { get; private set; } = Color.green;
    [field: SerializeField] public Color CellUnavailableColor { get; private set; } = Color.grey;
    [field: SerializeField] public Color CellRegularColor { get; private set; } = Color.red;
    [field: SerializeField] public Color CellOccupiedColor { get; private set; } = Color.grey;
    [field: SerializeField] public Color CellTargetedColor { get; private set; } = Color.red;
}
