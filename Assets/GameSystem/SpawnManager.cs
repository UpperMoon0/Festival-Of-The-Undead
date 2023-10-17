using Mirror;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    private GameObject[] enemyPrefabs;
    private GameObject[] buildingPrefabs;
    private FOTUNetworkManager networkManager;
    private int numberOfZombies = 10;
    private int spawnRadius = 20;

    public override void OnStartServer()
    {
        networkManager = GameObject.Find("Network Manager").GetComponent<FOTUNetworkManager>();
        enemyPrefabs = networkManager.enemyPrefabs;
        buildingPrefabs = networkManager.buildingPrefabs;

        for (int i = 0; i < numberOfZombies; i++)
        {
            SpawnZombie();
        }
    }

    private void SpawnZombie()
    {
        Vector2 spawnPosition;

        do
        {
            spawnPosition = Random.insideUnitCircle * spawnRadius;
        }
        while ((spawnPosition.x > -10.5f && spawnPosition.x < 10.5f && spawnPosition.y > -5f && spawnPosition.y < 5f) ||
                (spawnPosition.x < -spawnRadius || spawnPosition.x > spawnRadius || spawnPosition.y < -spawnRadius || spawnPosition.y > spawnRadius));

        GameObject zombie = Instantiate(enemyPrefabs[0], spawnPosition, Quaternion.identity);
        NetworkServer.Spawn(zombie);
    }

    public GameObject GetBuildingByID(int id)
    {
        foreach (GameObject building in buildingPrefabs)
        {
            if (building.GetComponent<Building>().id == id)
            {
                return building;
            }
        }

        return null;
    }
}
