using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : NetworkBehaviour
{
    private FOTUNetworkManager networkManager;
    private GameObject[] buildingPrefabs;
    private GameObject[] resourcesPrefab;
    private GameObject[] projectilesPrefab;
    private Dictionary<int, int> buildingToProjectile = new Dictionary<int, int>();

    void Start()
    {
        networkManager = GameObject.Find("Network Manager").GetComponent<FOTUNetworkManager>();
        buildingPrefabs = networkManager.buildingPrefabs;
        projectilesPrefab = networkManager.projectilesPrefab;
        resourcesPrefab = networkManager.resourcesPrefab;

        // Guard Tower - Guard Tower Projectile
        buildingToProjectile.Add(4, 0);
    }

    public GameObject GetTileByID(int id) {
        foreach (GameObject tile in buildingPrefabs)
        {
            if (tile.GetComponent<Tile>().ID == id)
            {
                return tile;
            }
        }

        foreach (GameObject tile in resourcesPrefab)
        {
            if (tile.GetComponent<Tile>().ID == id)
            {
                return tile;
            }
        }

        return null;
    }

    public GameObject GetProjectileByBuilding(Building building)
    {
        int id = building.ID;
        int projectileID;

        if (buildingToProjectile.TryGetValue(id, out projectileID))
        {
            foreach (GameObject projectile in projectilesPrefab)
            {
                if (projectile.GetComponent<Projectile>().ID == projectileID)
                {
                    return projectile;
                }
            }
        } 

        return null;
    }
}
