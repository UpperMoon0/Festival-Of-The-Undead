using Mirror;
using System;
using UnityEngine;

public class PlayerBuildingControl : NetworkBehaviour
{
    [SerializeField] private WorldManager s_worldManager;
    [SerializeField] private GameObject c_buildingGhost;
    [SerializeField] [SyncVar] private Vector3 buildingGhostPos;
    [SerializeField] [SyncVar] private bool canPlace = true;

    void Start()
    {
        if (isServer)
        {
            GameObject gameManager = GameObject.Find("Game Manager");
            s_worldManager = gameManager.GetComponent<WorldManager>();
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (c_buildingGhost != null)
            {
                BuildingGhost buildingGhostScript = c_buildingGhost.GetComponent<BuildingGhost>();

                // Get the mouse position in world space
                Camera playerCamera = transform.GetChild(1).GetComponent<Camera>();
                Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);

                // Update the building ghost position
                CmdUpdateBuildingGhostPos(mousePos);
                buildingGhostScript.MoveToPos(buildingGhostPos);

                // Update the canPlace variable
                CmdUpdateCanPlace(buildingGhostPos);
                buildingGhostScript.CanPlace(canPlace);

                // Place the building
                if (Input.GetMouseButtonDown(0) && canPlace)
                {
                    CmdPlaceBuilding(buildingGhostScript.GetBuildingPrefab().GetComponent<Building>().id, buildingGhostPos);    
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) CmdInteractWithBuilding();
        }
    }

    public void SetBuildingGhost(GameObject buildingGhost) => c_buildingGhost = buildingGhost;

    public GameObject GetBuildingGhost() => c_buildingGhost;

    public Vector3 GetBuildingGhostPos() => buildingGhostPos;

    public void DestroyBuildingGhost() => Destroy(c_buildingGhost);

    [Command]
    private void CmdUpdateBuildingGhostPos(Vector3 mousePos)
    {
        Vector2Int gridPosition = s_worldManager.WorldToGrid(mousePos);
        buildingGhostPos = s_worldManager.GridToWorld(gridPosition);
    }

    [Command]
    private void CmdUpdateCanPlace(Vector3 pos)
    {
        canPlace = s_worldManager.CanPlaceBuilding(pos);
    }

    [Command]
    public void CmdPlaceBuilding(int buildingID, Vector3 pos)
    {
        s_worldManager.PlaceBuilding(buildingID, pos);
    }

    [Command]
    public void CmdInteractWithBuilding()
    {
        // Define the interaction radius
        float interactionRadius = 2f;

        // Get all InteractableBuilding objects within the interaction radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

        // Sort hitColliders by distance to player
        Array.Sort(hitColliders, (a, b) => ((Vector2)(a.transform.position - transform.position)).sqrMagnitude.CompareTo(((Vector2)(b.transform.position - transform.position)).sqrMagnitude));

        foreach (var hitCollider in hitColliders)
        {
            InteractableBuilding building = hitCollider.GetComponent<InteractableBuilding>();
            if (building != null)
            {
                // Interact with the building
                building.Interact();
                break;
            }
        }
    }
}
