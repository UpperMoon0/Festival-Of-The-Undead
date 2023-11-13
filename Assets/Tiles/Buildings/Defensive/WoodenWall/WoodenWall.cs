public class WoodenWall : DefensiveBuilding
{
    public override int ID => 0;

    public override string TileName => "Wooden Wall";

    public override int Price => 50;

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
