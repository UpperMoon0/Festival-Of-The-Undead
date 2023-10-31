using Mirror;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] buildingPrefabs;
    [SerializeField] private GameObject[] resourcesPrefab;
    [SerializeField] private FOTUNetworkManager networkManager;
    [SerializeField] private WorldManager worldManager;
    [SerializeField] private int numberOfZombies = 10;
    [SerializeField] private int spawnRadius = 20;

    public override void OnStartServer()
    {
        networkManager = GameObject.Find("Network Manager").GetComponent<FOTUNetworkManager>();
        worldManager = GameObject.Find("Game Manager").GetComponent<WorldManager>();
        enemyPrefabs = networkManager.enemyPrefabs;
        buildingPrefabs = networkManager.buildingPrefabs;
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
        float gridSize = worldManager.GridSize;
        int worldWidth = worldManager.WorldWidth;
        int worldHeight = worldManager.WorldHeight;
        Vector2 spawnPosition = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));

        GameObject goldOre = resourcesPrefab[0];
        worldManager.PlaceTile(goldOre.GetComponent<Tile>().id, spawnPosition);
    }
    public GameObject GetTileByID(int id)
    {
        foreach (GameObject tile in buildingPrefabs)
        {
            if (tile.GetComponent<Tile>().id == id)
            {
                return tile;
            }
        }

        foreach (GameObject tile in resourcesPrefab)
        {
            if (tile.GetComponent<Tile>().id == id)
            {
                return tile;
            }
        }

        return null;
    }
}
