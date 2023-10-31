using Mirror;
using System;
using UnityEngine;

public class PlayerBuildControl : NetworkBehaviour
{
    [SerializeField] private WorldManager s_WorldManager;

    [SerializeField] private GameObject c_BuildingGhost;

    [SerializeField] private Player c_player;

    [SerializeField] [SyncVar] private Vector3 buildingGhostPos;
    [SerializeField] [SyncVar] private bool canPlace = true;


    void Start()
    {
        if (isLocalPlayer)
        {
            c_player = GetComponent<Player>();
        }


        if (isServer)
        {
            GameObject gameManager = GameObject.Find("Game Manager");
            s_WorldManager = gameManager.GetComponent<WorldManager>();
        } 
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (c_BuildingGhost != null)
            {
                BuildingGhost buildingGhostScript = c_BuildingGhost.GetComponent<BuildingGhost>();

                // Get the mouse position in world space
                Camera playerCamera = transform.parent.GetChild(2).GetComponent<Camera>();
                Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);

                // Update the building ghost position
                CmdUpdateBuildingGhostPos(mousePos);
                buildingGhostScript.MoveToPos(buildingGhostPos);

                // Update the canPlace variable
                int buildingID = buildingGhostScript.GetBuildingPrefab().GetComponent<Tile>().id;
                CmdUpdateCanPlace(buildingID, buildingGhostPos);
                buildingGhostScript.CanPlace(canPlace);

                // Place the building
                if (Input.GetMouseButtonDown(0) && canPlace)
                {
                    CmdPlaceBuilding(buildingID, buildingGhostPos, c_player);    
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) CmdInteractWithBuilding();
        }
    }

    public void SetBuildingGhost(GameObject buildingGhost) => c_BuildingGhost = buildingGhost;

    public GameObject GetBuildingGhost() => c_BuildingGhost;

    public Vector3 GetBuildingGhostPos() => buildingGhostPos;

    public void DestroyBuildingGhost() => Destroy(c_BuildingGhost);

    [Command]
    private void CmdUpdateBuildingGhostPos(Vector3 mousePos)
    {
        Vector2Int gridPosition = s_WorldManager.WorldToGrid(mousePos);
        buildingGhostPos = s_WorldManager.GridToWorld(gridPosition);
    }

    [Command]
    private void CmdUpdateCanPlace(int buildingID, Vector3 pos)
    {
        canPlace = s_WorldManager.CanPlaceTile(buildingID, pos);
    }

    [Command]
    private void CmdPlaceBuilding(int buildingID, Vector3 pos, Player owner)
    {
        s_WorldManager.PlaceTile(buildingID, pos, owner);
    }

    [Command]
    private void CmdInteractWithBuilding()
    {
        // Define the interaction radius
        float interactionRadius = 2f;

        // Get all IInteractableBuilding objects within the interaction radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

        // Sort hitColliders by distance to c_player
        Array.Sort(hitColliders, (a, b) => ((Vector2)(a.transform.position - transform.position)).sqrMagnitude.CompareTo(((Vector2)(b.transform.position - transform.position)).sqrMagnitude));

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
