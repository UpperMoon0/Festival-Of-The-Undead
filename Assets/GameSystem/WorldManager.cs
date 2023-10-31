using Mirror;
using UnityEngine;

public class WorldManager : NetworkBehaviour 
{ 
    private Tile[,] grid;
    private float gridSize = 1f;
    private int worldWidth = 100;
    private int worldHeight = 100;

    public float GridSize
    {
        get { return gridSize; }
    }

    public int WorldWidth
    {
        get { return worldWidth; }
    }

    public int WorldHeight
    {
        get { return worldHeight; }
    }

    void Awake()
    {
        grid = new Tile[worldWidth, worldHeight];
    }

    public bool CanPlaceTile(int tileID, Vector2 position)
    {
        Vector2Int gridPosition = WorldToGrid(position);
        int gridX = gridPosition.x;
        int gridY = gridPosition.y;

        if (OutOfGridBound(gridX, gridY))
        {
            return false;
        }

        // Check if tile is a mine
        if (tileID == 3)
        {
            if (GetTileAtGrid(gridX,gridY) is ResourceTile)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return grid[gridX, gridY] == null;
    }

    public void PlaceTile(int tileID, Vector2 pos)
    {
        Vector2Int gridPosition = WorldToGrid(pos);
        int gridX = gridPosition.x;
        int gridY = gridPosition.y;

        if (OutOfGridBound(gridX, gridY))
        {
            return;
        }

        GameObject tile = Instantiate(GetComponent<SpawnManager>().GetTileByID(tileID), pos, Quaternion.identity);
        NetworkServer.Spawn(tile);
        grid[gridX, gridY] = tile.GetComponent<Tile>();
    }

    public void PlaceTile(int buildingID, Vector2 pos, Player owner)
    {
        Vector2Int gridPosition = WorldToGrid(pos);
        int gridX = gridPosition.x;
        int gridY = gridPosition.y;

        if (OutOfGridBound(gridX, gridY))
        {
            return;
        }

        if (buildingID == 3)
        {
            Tile resourceTile = GetTileAtGrid(gridX, gridY);

            if (resourceTile is ResourceTile)
            {
                resourceTile.DestroyTile();
            }
        }

        GameObject tile = Instantiate(GetComponent<SpawnManager>().GetTileByID(buildingID), pos, Quaternion.identity);
        NetworkServer.Spawn(tile);
        tile.GetComponent<Building>().Owner = owner;
        grid[gridX, gridY] = tile.GetComponent<Tile>();
    }


    public void RemoveTile(Vector2 pos)
    {
        Vector2Int gridPosition = WorldToGrid(pos);
        int gridX = gridPosition.x;
        int gridY = gridPosition.y;

        if (!OutOfGridBound(gridX, gridY))
        {
            grid[gridX, gridY] = null;
        }
    }

    public Vector2Int WorldToGrid(Vector2 worldPos)
    {
        int x = Mathf.RoundToInt((worldPos.x + grid.GetLength(0) * gridSize / 2) / gridSize);
        int y = Mathf.RoundToInt((worldPos.y + grid.GetLength(1) * gridSize / 2) / gridSize);
        return new Vector2Int(x, y);
    }

    public Vector2 GridToWorld(Vector2Int gridPos)
    {
        float x = (gridPos.x * gridSize) - grid.GetLength(0) * gridSize / 2;
        float y = (gridPos.y * gridSize) - grid.GetLength(1) * gridSize / 2;
        return new Vector2(x, y);
    }

    public Tile GetTileAtGrid(int gridX, int gridY)
    {
        if (OutOfGridBound(gridX, gridY))
        {
            return null;
        }

        return grid[gridX, gridY];
    }

    public bool TileEmpty(int gridX, int gridY)
    {
        if (OutOfGridBound(gridX, gridY))
        {
            return false;
        }

        return grid[gridX, gridY] == null;
    }

    private bool OutOfGridBound(int gridX, int gridY)
    {
        return gridX < 0 || gridX >= grid.GetLength(0) || gridY < 0 || gridY >= grid.GetLength(1);
    }
}