using Mirror;
using UnityEngine;

public abstract class DestructibleBuilding : Building
{
    [SerializeField] [SyncVar] private int svMaxHealth;
    [SerializeField] [SyncVar] private int currentHealth;

    protected int maxHealth
    {
        get { return svMaxHealth; }
        set { svMaxHealth = value; }
    }

    protected override void StartExtension()
    {
        base.StartExtension();

        gameObject.tag = "DestructibleBuilding";

        if (isServer)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (isServer)
        {
            if (currentHealth <= 0)
            {
                NetworkServer.Destroy(gameObject);
            }
        }
    }
}

