using UnityEngine;

public class WoodenWall : DefensiveBuilding
{
    public override int id => 0;

    public override string tileName => "Wooden Wall";

    public override int price => 50;

    protected override void StartExtension()
    {
        base.StartExtension();

        if (isServer)
        {
            maxHealth = 1000;
            currentHealth = maxHealth;
        }
    }
}
