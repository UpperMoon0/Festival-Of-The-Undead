using Mirror;
using UnityEngine;

public class WorldManager : NetworkBehaviour 
{ 
    private Building[,] grid;
    private float gridSize = 1f;

    void Start()
    {
        grid = new Building[100, 100];
    }

    public bool CanPlaceBuilding(Vector3 position)
    {
        Vector2Int gridPosition = WorldToGrid(position);
        int gridX = gridPosition.x;
        int gridY = gridPosition.y;

        if (gridX < 0 || gridX >= grid.GetLength(0) || gridY < 0 || gridY >= grid.GetLength(1))
        {
            return false;
        }

        return grid[gridX, gridY] == null;
    }

    public void PlaceBuilding(int buildingID, Vector3 position)
    {
        if (CanPlaceBuilding(position))
        {
            GameObject building = Instantiate(GetComponent<SpawnManager>().GetBuildingByID(buildingID), position, Quaternion.identity);
            NetworkServer.Spawn(building);

            Vector2Int gridPosition = WorldToGrid(position);
            int gridX = gridPosition.x;
            int gridY = gridPosition.y;

            grid[gridX, gridY] = building.GetComponent<Building>();
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x + grid.GetLength(0) * gridSize / 2) / gridSize);
        int y = Mathf.RoundToInt((worldPosition.y + grid.GetLength(1) * gridSize / 2) / gridSize);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        float x = (gridPosition.x * gridSize) - grid.GetLength(0) * gridSize / 2;
        float y = (gridPosition.y * gridSize) - grid.GetLength(1) * gridSize / 2;
        return new Vector3(x, y, 0);
    }
}