using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIControl : MonoBehaviour
{

    public GameObject buildingButtonPrefab;
    public GameObject buildingGhostPrefab;

    private GameObject[] buildingPrefabs;
    private PlayerBuildingControl buildingControlScript;
    private GameObject[] buildingButtons;
    private GameObject buildMenu;
    private GameObject buildingList;

    void Start()
    {
        FOTUNetworkManager networkManager = GameObject.Find("Network Manager").GetComponent<FOTUNetworkManager>(); 
        buildingPrefabs = networkManager.buildingPrefabs;

        buildingControlScript = GetComponent<PlayerBuildingControl>();

        buildingButtons = new GameObject[buildingPrefabs.Length];
        buildMenu = transform.GetChild(2).GetChild(0).gameObject;
        buildingList = buildMenu.transform.GetChild(0).gameObject;

        for (int i = 0; i < buildingPrefabs.Length; i++)
        {
            GameObject buildingButton = Instantiate(buildingButtonPrefab, buildingList.transform);
            buildingButton.GetComponent<Image>().sprite = buildingPrefabs[i].GetComponent<SpriteRenderer>().sprite;

            // Create a temporary variable to capture the current value of i
            int index = i;
            buildingButton.GetComponent<Button>().onClick.AddListener(delegate { CreateBuildingGhost(buildingPrefabs[index]); });

            buildingButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = buildingPrefabs[i].GetComponent<Building>().buildingName;
            buildingButtons[i] = buildingButton;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildMenu.SetActive(!buildMenu.activeSelf);
        }
    }

    public void CreateBuildingGhost(GameObject buildingPrefab)
    {
        if (buildingControlScript.GetBuildingGhost() != null)
        {
            buildingControlScript.DestroyBuildingGhost();

            if (buildingPrefab.Equals(buildingControlScript.GetBuildingGhost().GetComponent<BuildingGhost>().GetBuildingPrefab())) return;
        }

        buildingControlScript.SetBuildingGhost(Instantiate(buildingGhostPrefab, buildingControlScript.GetBuildingGhostPos(), Quaternion.identity));
        BuildingGhost buildingGhostScript = buildingControlScript.GetBuildingGhost().GetComponent<BuildingGhost>();
        buildingGhostScript.InitSpriteRenderer();
        buildingGhostScript.SetBuilding(buildingPrefab);
    }
}
