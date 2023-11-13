using Mirror;
using UnityEngine;

public class LivingEntity : NetworkBehaviour
{
    [SerializeField] protected int maxHealth;
    [SerializeField] [SyncVar] protected int currentHealth;
    [SerializeField] protected Rigidbody2D rb;

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
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
