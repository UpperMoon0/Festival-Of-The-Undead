using UnityEngine;

public class Zombie : Enemy
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int dmg = 20;
    [SerializeField] private float attackCD = 1.5f;
    [SerializeField] private float lastAttackTime;

    protected override void StartExtension()
    {
        base.StartExtension();

        maxHealth = 100;
        currentHealth = maxHealth;
    }

    protected override void UpdateExtension()
    {
        base.UpdateExtension();

        if (isServer)
        {
            GameObject closestPlayer = FindClosestPlayer();
            GameObject closestBuilding = FindClosestBuilding();

            if (closestBuilding != null && Vector2.Distance(transform.position, closestBuilding.transform.position) <= attackRange)
            {
                if (Time.time >= lastAttackTime + attackCD)
                {
                    AttackBuilding(closestBuilding);
                    lastAttackTime = Time.time;
                }
            }
            else if (closestPlayer != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, closestPlayer.transform.position, speed * Time.deltaTime);
            }
        }
    }

    void AttackBuilding(GameObject building)
    {
        Building destructibleBuilding = building.GetComponent<Building>();
        if (destructibleBuilding != null)
        {
            destructibleBuilding.TakeDamage(dmg);
        }
    }

    GameObject FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector2 currentPosition = transform.position;

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(player.transform.position, currentPosition);
            if (distance < minDistance)
            {
                closest = player;
                minDistance = distance;
            }
        }

        return closest;
    }

    GameObject FindClosestBuilding()
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("DestructibleBuilding");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector2 currentPosition = transform.position;

        foreach (GameObject building in buildings)
        {
            float distance = Vector2.Distance(building.transform.position, currentPosition);
            if (distance < minDistance)
            {
                closest = building;
                minDistance = distance;
            }
        }

        return closest;
    }
}
