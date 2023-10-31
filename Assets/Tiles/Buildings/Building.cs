using Mirror;
using UnityEngine;

public abstract class Building : Tile
{

    [SerializeField] [SyncVar] protected int maxHealth;
    [SerializeField] [SyncVar] protected int currentHealth;
    [SerializeField] [SyncVar] protected Player owner;
    [SerializeField] public abstract int type { get; }

    public Player Owner
    {
        get { return owner; }
        set { owner = value; }
    }

    protected override void StartExtension()
    {
        base.StartExtension();

        gameObject.tag = "DestructibleBuilding";
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

