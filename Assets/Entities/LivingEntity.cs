using Mirror;
using UnityEngine;

public class LivingEntity : NetworkBehaviour
{
    [SerializeField] [SyncVar] protected int maxHealth;
    [SerializeField] [SyncVar] protected int currentHealth;

    private void Awake()
    {
        AwakeExtension();
    }

    private void Start()
    { 
        StartExtension();
    }

    private void Update()
    {
        UpdateExtension();
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
