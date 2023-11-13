using Mirror;
using System;
using UnityEngine;

public class PlayerBuildControl : NetworkBehaviour
{
    [SerializeField] private GridManager s_GridManager;
    [SerializeField] private TileManager s_TileManager;
    [SerializeField] private Player s_Player;

    [SerializeField] private GameObject c_BuildingGhostObj;
    [SerializeField] private bool c_DemolishMode = false;

    [SerializeField] [SyncVar] private Vector3 buildingGhostPos;
    [SerializeField] [SyncVar] private bool canPlace = false;
    [SerializeField] [SyncVar] private bool canDemolish = false;

    public bool C_DemolishMode { get => c_DemolishMode; set => c_DemolishMode = value; }

    void Start()
    {
        if (isServer)
        {
            GameObject gameManager = GameObject.Find("Game Manager");
            s_TileManager = gameManager.GetComponent<TileManager>();
            s_GridManager = gameManager.GetComponent<GridManager>();
            s_Player = GetComponent<Player>();
        } 
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (c_BuildingGhostObj != null)
            {
                BuildingGhost buildingGhost = c_BuildingGhostObj.GetComponent<BuildingGhost>();

                // Get the mouse position in world space
                Camera playerCamera = transform.parent.GetChild(2).GetComponent<Camera>();
                Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);

                // Update the building ghost position
                CmdUpdateBuildingGhostPos(mousePos);
                buildingGhost.MoveToPos(buildingGhostPos);
                int buildingID = -1;
                // Update the canPlace variable
                if (c_DemolishMode)
                {
                    CmdUpdateCanDemolish(mousePos);
                    buildingGhost.CanPlace(canDemolish);
                } else {
                    buildingID = buildingGhost.GetBuildingPrefab().GetComponent<Tile>().ID;
                    CmdUpdateCanPlace(buildingID, buildingGhostPos);
                    buildingGhost.CanPlace(canPlace);
                }

                // Place the building
                if (Input.GetMouseButtonDown(0))
                {
                    if (!c_DemolishMode && canPlace)
                    {
                        CmdPlaceBuilding(buildingID, buildingGhostPos);
                    }

                    if (c_DemolishMode && canDemolish)
                    {
                        CmdDemolishBuilding(buildingGhostPos);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) CmdInteractWithBuilding();
        }
    }

    public void SetBuildingGhost(GameObject buildingGhost) => c_BuildingGhostObj = buildingGhost;

    public GameObject GetBuildingGhost() => c_BuildingGhostObj;

    public Vector3 GetBuildingGhostPos() => buildingGhostPos;

    public void DestroyBuildingGhost() => Destroy(c_BuildingGhostObj);

    [Command]
    private void CmdUpdateBuildingGhostPos(Vector3 mousePos)
    {
        Vector2Int gridPosition = s_GridManager.WorldToGrid(mousePos);
        buildingGhostPos = s_GridManager.GridToWorld(gridPosition);
    }

    [Command]
    private void CmdUpdateCanPlace(int buildingID, Vector3 pos)
    {
        bool condition1 = s_GridManager.CanPlaceTile(buildingID, pos);
        bool condition2 = s_Player.Currency >= ((Building) s_TileManager.GetTileByID(buildingID).GetComponent<Tile>()).Price;
        canPlace = condition1 && condition2;
    }

    [Command]
    private void CmdUpdateCanDemolish(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        GameObject obj = hit.collider?.gameObject;

        canDemolish = false;

        if (obj != null)
        {
            Building building = obj.GetComponent<Building>();

            if (building != null)
            {
                Player player = GetComponent<Player>();
                canDemolish = s_GridManager.CanRemoveTile(building, player, pos);
            }
        }
    }

    [Command]
    private void CmdPlaceBuilding(int buildingID, Vector3 pos)
    {
        s_GridManager.PlaceTile(buildingID, pos, s_Player);
        s_Player.Currency -= ((Building) s_TileManager.GetTileByID(buildingID).GetComponent<Tile>()).Price;
    }

    [Command]
    private void CmdDemolishBuilding(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        GameObject obj = hit.collider?.gameObject;

        if (obj != null && obj.GetComponent<Building>() != null)
        {
            s_GridManager.RemoveTile(obj, pos);
        }
    }

    [Command]
    private void CmdInteractWithBuilding()
    {
        // Define the interaction radius
        float interactionRadius = 2f;

        // Get all IInteractableBuilding objects within the interaction radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

        // Sort hitColliders by distance to c_player
        Array.Sort(hitColliders, (a, b) => 
            ((Vector2)(a.transform.position - transform.position)).sqrMagnitude.CompareTo(((Vector2)(b.transform.position - transform.position)).sqrMagnitude));

        foreach (var hitCollider in hitColliders)
        {
            IInteractableBuilding building = hitCollider.GetComponent<IInteractableBuilding>();
            if (building != null)
            {
                // Interact with the building
                building.Interact();
                break;
            }
        }
    }
}
