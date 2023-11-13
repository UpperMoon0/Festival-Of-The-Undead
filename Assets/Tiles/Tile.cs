using Mirror;
using UnityEngine;

public abstract class Tile : NetworkBehaviour
{
    [SerializeField] public abstract int ID { get; }
    [SerializeField] public abstract string TileName { get; }

    [SerializeField] protected GameObject s_GameManager;
    [SerializeField] protected GridManager s_GridManager;

    private void Awake()
    { 
        AwakeExtension();
    }

    private void Start()
    {
        if (isServer)
        {
            s_GameManager = GameObject.Find("Game Manager");
            s_GridManager = s_GameManager.GetComponent<GridManager>();
        }
        StartExtension();
    }

    private void Update()
    {
        UpdateExtension();
    }

    public void DestroyTile()
    {
        s_GridManager.RemoveTile(gameObject ,transform.position);
    }

    protected virtual void StartExtension()
    {

    }

    protected virtual void AwakeExtension()
    {

    }

    protected virtual void UpdateExtension()
    {

    }
}
