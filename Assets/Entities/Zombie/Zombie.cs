using Mirror;
using UnityEngine;

public class Zombie : NetworkBehaviour
{
    private float speed = 1f; // adjust to desired speed
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    void Update()
    {
        if (!isServer) // only run on server
            return;

        GameObject closestPlayer = FindClosestPlayer();
        if (closestPlayer != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestPlayer.transform.position, speed * Time.deltaTime);
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
}
