using Mirror;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    public GameObject zombiePrefab; 
    private int numberOfZombies = 10;
    private int spawnRadius = 20;

    public override void OnStartServer()
    {
        for (int i = 0; i < numberOfZombies; i++)
        {
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        Vector2 spawnPosition;

        do
        {
            spawnPosition = Random.insideUnitCircle * spawnRadius;
        }
        while ((spawnPosition.x > -10.5f && spawnPosition.x < 10.5f && spawnPosition.y > -5f && spawnPosition.y < 5f) ||
               (spawnPosition.x < -spawnRadius || spawnPosition.x > spawnRadius || spawnPosition.y < -spawnRadius || spawnPosition.y > spawnRadius));

        GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        NetworkServer.Spawn(zombie);
    }

}
