using Mirror;
using UnityEngine;

public class Zombie : NetworkBehaviour
{
    private float speed = 1f; 
    private float attackRange = 1f;
    private int attackDamage = 20; 
    private float attackCooldown = 1.5f;
    private float lastAttackTime;
    private Rigidbody2D rb;

    void Start()
    {
        if (!isServer) return;

        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (!isServer) return;

        GameObject closestPlayer = FindClosestPlayer();
        GameObject closestBuilding = FindClosestBuilding();

        if (closestBuilding != null && Vector2.Distance(transform.position, closestBuilding.transform.position) <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
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

    [Server]
    void AttackBuilding(GameObject building)
    {
        Building destructibleBuilding = building.GetComponent<Building>();
        if (destructibleBuilding != null)
        {
            destructibleBuilding.TakeDamage(attackDamage);
        }
    }

    [Server]
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

    [Server]
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
