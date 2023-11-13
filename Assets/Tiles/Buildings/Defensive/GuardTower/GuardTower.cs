using Mirror;
using UnityEngine;
using System.Collections;

public class GuardTower : DefensiveBuilding
{
    private float attackRange = 4.5f;
    private float shootCooldown = 1.0f;
    private float lastShootTime = 0.0f;

    public override int ID => 4;

    public override string TileName => "Guard Tower";

    public override int Price => 200;

    protected override void StartExtension()
    {
        base.StartExtension();

        if (isServer)
        {
            maxHealth = 500;
            currentHealth = maxHealth;
        }
    }

    protected override void UpdateExtension()
    {
        if (isServer)
        {
            // Check if there are any enemies in range
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.tag == "Enemy")
                {
                    // Check if enough time has passed since the last shot
                    if (Time.time - lastShootTime >= shootCooldown)
                    {
                        Vector2 projPos = new Vector2(transform.position.x, transform.position.y + 2);
                        Vector2 projDir = ((Vector2)collider.transform.position - projPos).normalized;
                        GameObject projObj = Instantiate(tileManager.GetProjectileByBuilding(GetComponent<Building>()), projPos, Quaternion.identity);
                        GuardTowerProj proj = projObj.GetComponent<GuardTowerProj>();
                        proj.MaxDistance = attackRange * 2;
                        proj.Dir = projDir;
                        NetworkServer.Spawn(projObj);

                        // Update the last shoot time
                        lastShootTime = Time.time;
                    }
                }
            }
        }
    }
}
