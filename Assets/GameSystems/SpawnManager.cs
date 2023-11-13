using Mirror;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] resourcesPrefab;
    [SerializeField] private FOTUNetworkManager networkManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private int numberOfZombies = 10;
    [SerializeField] private int spawnRadius = 20;

    void Start()
    {
        if (isServer)
        {
            networkManager = GameObject.Find("Network Manager").GetComponent<FOTUNetworkManager>();
            gridManager = GetComponent<GridManager>();
            enemyPrefabs = networkManager.enemyPrefabs;
            resourcesPrefab = networkManager.resourcesPrefab;

            for (int i = 0; i < numberOfZombies; i++)
            {
                SpawnZombie();
            }

            for (int i = 0; i < 30; i++)
            {
                SpawnGoldPatch();
            }
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
    
    private void SpawnGoldPatch()
    {
        float gridSize = gridManager.GridSize;
        int worldWidth = gridManager.WorldWidth;
        int worldHeight = gridManager.WorldHeight;
        Vector2 spawnPosition = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));

        GameObject goldOre = resourcesPrefab[0];
        gridManager.PlaceTile(goldOre.GetComponent<Tile>().ID, spawnPosition);
    }
}
