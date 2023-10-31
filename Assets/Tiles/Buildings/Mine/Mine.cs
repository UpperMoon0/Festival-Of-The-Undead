using UnityEngine;

public class Mine : EconomyBuilding
{
    [SerializeField] private int currencyTickAmount = 25;

    public override int id => 3;

    public override string tileName => "Mine";

    public override int price => 100;

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
