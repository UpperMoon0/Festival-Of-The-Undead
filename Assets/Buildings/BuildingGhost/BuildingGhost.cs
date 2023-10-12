using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private Camera mainCamera;
    public float gridSize = 1f; // Set this to the size of your grid

    public void SetCamera(Camera camera)
    {
        mainCamera = camera;
    }

    void Update()
    {
        // Convert the mouse position to world position
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the nearest grid point
        float x = Mathf.Round(mousePosition.x / gridSize) * gridSize;
        float y = Mathf.Round(mousePosition.y / gridSize) * gridSize;

        // Set the position of the building ghost to the nearest grid point
        // We keep the z-position the same to not move the object closer or farther from the camera
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
