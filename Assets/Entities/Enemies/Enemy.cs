public class Enemy : LivingEntity
{
    protected override void StartExtension()
    {
        base.StartExtension();

        gameObject.tag = "Enemy";
    }
}
