using Mirror;
using UnityEngine;

public class Player : LivingEntity
{
    [SerializeField] [SyncVar] private int currency;
    [SerializeField] private float s_CurrencyTickRate = 1f;
    [SerializeField] private float s_CurrencyLastTick = 0f;
    [SerializeField] private int s_CurrencyTickAmount = 5;

    public int Currency
    {
        get { return currency; }
        set { currency = value; }
    }

    public int CurrencyTickAmount
    {
        get { return s_CurrencyTickAmount; }
        set { s_CurrencyTickAmount = value; }
    }

    protected override void StartExtension()
    {
        base.StartExtension();

        maxHealth = 100;
        currentHealth = maxHealth;
        currency = 0;
    }

    protected override void UpdateExtension()
    {
        base.UpdateExtension();

        if (isServer)
        {
            if (Time.time - s_CurrencyLastTick >= s_CurrencyTickRate)
            {
                s_CurrencyLastTick = Time.time;
                currency += s_CurrencyTickAmount;
            }
        }
    }
}
