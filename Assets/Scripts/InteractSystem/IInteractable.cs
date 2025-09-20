public interface IInteractable
{
    public T GetData<T>() where T : ObjectData;

    public bool IsInteractable { get; }

    public void OnPointEnter();
    public void OnPointExit();
    public void Interact();
}

public enum ObjectType
{
    Unit,
    Table,
    Chair,
    Decor,
    Cell
}