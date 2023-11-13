using Mirror;
using UnityEngine;

public abstract class Building : Tile
{
    [SerializeField] public abstract int Type { get; }
    [SerializeField] public abstract int Price { get; }

    [SerializeField] [SyncVar] protected int maxHealth;
    [SerializeField] [SyncVar] protected int currentHealth;

    [SerializeField] protected Player owner;
    [SerializeField] protected TileManager tileManager;
    public Player Owner
    {
        get { return owner; }
        set { owner = value; }
    }

    protected override void StartExtension()
    {
        base.StartExtension();

        if (isServer)
        {
            tileManager = s_GameManager.GetComponent<TileManager>();
            gameObject.tag = "DestructibleBuilding";
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (isServer)
        {
            if (currentHealth <= 0)
            {
                OnDestroyExtension();
                DestroyTile();
            }
        }
    }

    public virtual void OnDestroyExtension()
    {

    }
}

