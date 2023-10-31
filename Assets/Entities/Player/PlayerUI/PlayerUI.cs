using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    public GameObject buildingButtonPrefab;
    public GameObject buildingGhostPrefab;

    private GameObject[] buildingPrefabs;
    private GameObject[] buildingButtons;
    private GameObject buildMenu;
    private GameObject playerHUD;
    private GameObject currencyBar;
    private GameObject settingButton;
    private GameObject quitButton;

    private Player playerScript;
    private PlayerBuildControl buildingControlScript;

    private bool isSetting = false;

    private void Start()
    {
        // Initialize the variables
        FOTUNetworkManager networkManager = GameObject.Find("Network Manager").GetComponent<FOTUNetworkManager>(); 
        buildingPrefabs = networkManager.buildingPrefabs;
        playerScript = GetComponent<Player>();
        buildingControlScript = GetComponent<PlayerBuildControl>();
        buildingButtons = new GameObject[buildingPrefabs.Length];
        GameObject canvas = transform.parent.GetChild(3).gameObject;
        buildMenu = canvas.transform.GetChild(0).gameObject;
        GameObject buildingList = buildMenu.transform.GetChild(0).gameObject;
        playerHUD = canvas.transform.GetChild(1).gameObject;
        currencyBar = playerHUD.transform.GetChild(2).gameObject;
        GameObject settingHUD = canvas.transform.GetChild(2).gameObject;
        settingButton = settingHUD.transform.GetChild(0).gameObject;
        quitButton = settingHUD.transform.GetChild(1).gameObject;

        // Create the building buttons
        for (int i = 0; i < buildingPrefabs.Length; i++)
        {
            GameObject buildingButton = Instantiate(buildingButtonPrefab, buildingList.transform);
            buildingButton.GetComponent<Image>().sprite = buildingPrefabs[i].GetComponent<SpriteRenderer>().sprite;

            // Create a temporary variable to capture the current value of i
            int index = i;
            buildingButton.GetComponent<Button>().onClick.AddListener(delegate { CreateBuildingGhost(buildingPrefabs[index]); });

            buildingButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = buildingPrefabs[i].GetComponent<Tile>().tileName;
            buildingButtons[i] = buildingButton;
        }

        // Add listener to the setting buttons
        settingButton.GetComponent<Button>().onClick.AddListener(ToggleSetting);
        quitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (buildMenu.activeSelf)
            {
                buildMenu.SetActive(false);

                if (buildingControlScript.GetBuildingGhost() != null)
                {
                    buildingControlScript.DestroyBuildingGhost();
                }
            }
            else
            {
                buildMenu.SetActive(true);
            }
        }

        currencyBar.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerScript.Currency.ToString();
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

    public void ToggleSetting()
    {
        if (isSetting)
        {
            isSetting = false;
            quitButton.SetActive(false);
        }
        else
        {
            isSetting = true;
            quitButton.SetActive(true);
        }
    }

    public void QuitGame()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (!NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
    }
}
