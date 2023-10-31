using UnityEngine;

public class Mine : EconomyBuilding
{
    public override int id => 3;

    public override string tileName => "Mine";

    [SerializeField] private int currencyTickAmount = 25;

    protected override void StartExtension()
    {
        base.StartExtension();

        if (isServer)
        {
            maxHealth = 300;
            currentHealth = maxHealth;
            owner.CurrencyTickAmount += currencyTickAmount;
        }
    }

    public override void OnDestroyExtension()
    {
        base.OnDestroyExtension();

        if (isServer)
        {
            worldManager.PlaceTile(2, transform.position);
            owner.CurrencyTickAmount -= currencyTickAmount;
        }
    }
}
