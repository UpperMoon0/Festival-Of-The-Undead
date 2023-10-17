using Mirror;

public abstract class Building : NetworkBehaviour
{
    public abstract int id { get; }
    public abstract string buildingName { get; }

    private void Awake()
    { 
        AwakeExtension();
    }

    private void Start()
    {
        StartExtension();
    }

    protected virtual void StartExtension()
    {

    }

    protected virtual void AwakeExtension()
    {

    }
}
