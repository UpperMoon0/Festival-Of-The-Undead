using Mirror;
using UnityEngine;

public class PlayerBuildingControl : NetworkBehaviour
{
    public GameObject buildingGhostPrefab;
    public GameObject wallPrefab;
    private GameObject buildingGhost;

    void Update()
    {
        // Press B to toggle the building ghost
        if (Input.GetKeyDown(KeyCode.B))
        {
            // If there is already a building ghost, destroy it
            if (buildingGhost != null)
            {
                Destroy(buildingGhost);
                buildingGhost = null;
            }
            else
            {
                // If there is no building ghost, create one
                buildingGhost = Instantiate(buildingGhostPrefab, transform.position, Quaternion.identity);

                // Get the camera from this player's child objects
                Camera playerCamera = GetComponentInChildren<Camera>();

                // Set the camera for the building ghost
                BuildingGhost buildingGhostScript = buildingGhost.GetComponent<BuildingGhost>();
                buildingGhostScript.SetCamera(playerCamera);
            }
        }

        // Left click to build a wall at the position of the building ghost
        if (isLocalPlayer && Input.GetMouseButtonDown(0) && buildingGhost != null)
        {
            CmdPlaceBuilding(buildingGhost.transform.position);
        }
    }

    [Command]
    void CmdPlaceBuilding(Vector3 position)
    {
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
        NetworkServer.Spawn(wall);
    }

}
