using UnityEngine;

public class Mine : EconomyBuilding
{
    [SerializeField] private int currencyTickAmount = 25;

    public override int ID => 3;

    public override string TileName => "Mine";

    public override int Price => 100;

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
            s_GridManager.PlaceTile(2, transform.position);
            owner.CurrencyTickAmount -= currencyTickAmount;
        }
    }
}
