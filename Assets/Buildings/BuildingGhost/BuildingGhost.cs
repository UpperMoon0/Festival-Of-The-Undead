using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] private bool canPlace = true;

    void Update()
    {
        spriteRenderer.color = canPlace ? Color.white : Color.red;
    }

    public void InitSpriteRenderer() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetBuilding(GameObject buildingPrefab) 
    { 
        this.buildingPrefab = buildingPrefab; 
        spriteRenderer.sprite = buildingPrefab.GetComponent<SpriteRenderer>().sprite;
    }

    public GameObject GetBuildingPrefab() => buildingPrefab;

    public void CanPlace(bool canPlace) => this.canPlace = canPlace;

    public void MoveToPos(Vector3 pos) => transform.position = pos;
}
