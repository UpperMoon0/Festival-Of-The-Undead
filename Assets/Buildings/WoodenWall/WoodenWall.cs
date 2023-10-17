public class WoodenWall : DestructibleBuilding
{
    public override int id => 0;

    public override string buildingName => "Wooden Wall";

    protected override void StartExtension()
    {
        base.StartExtension();

        if (isServer)
        {
            maxHealth = 1000;
        }
    }
}
