using Mirror;
using UnityEngine;

public abstract class Tile : NetworkBehaviour
{
    public abstract int id { get; }
    public abstract string tileName { get; }

    protected WorldManager worldManager;

    private void Awake()
    { 
        AwakeExtension();
    }

    private void Start()
    {
        if (isServer)
        {
            worldManager = GameObject.Find("Game Manager").GetComponent<WorldManager>();
        }
        StartExtension();
    }

    public void DestroyTile()
    {
        worldManager.RemoveTile(transform.position);
        NetworkServer.Destroy(gameObject);
    }

    protected virtual void StartExtension()
    {

    }

    protected virtual void AwakeExtension()
    {

    }
}
