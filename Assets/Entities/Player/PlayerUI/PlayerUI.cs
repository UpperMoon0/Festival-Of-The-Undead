using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public GameObject buildingButtonPrefab;
    public GameObject buildingGhostPrefab;

    [SerializeField] private GameObject[] buildingPrefabs;
    private GameObject[] buildingButtons;
    private GameObject buildMenu;
    private GameObject playerHUD;
    private GameObject currencyBar;
    private GameObject settingButton;
    private GameObject quitButton;
    private GameObject defensiveBuildingList;
    private GameObject economyBuildingList;
    private GameObject defensiveBuildingButton;
    private GameObject economyBuildingButton;
    private GameObject demolishButton;

    private Player playerScript;
    private PlayerBuildControl buildingControlScript;

    private bool isSetting = false;

    private void Start()
    {
        InitializeVariables();
        CreateBuildingButtons();
        AddListenersToSettingButtons();
        EnableDefensiveTab();
    }

    private void Update()
    {
        ToggleBuildMenu();
        UpdateCurrencyBar();
    }

    private void InitializeVariables()
    {
        // Initialize the variables
        FOTUNetworkManager networkManager = GameObject.Find("Network Manager").GetComponent<FOTUNetworkManager>();
        buildingPrefabs = networkManager.buildingPrefabs;

        playerScript = GetComponent<Player>();
        buildingControlScript = GetComponent<PlayerBuildControl>();

        buildingButtons = new GameObject[buildingPrefabs.Length];

        GameObject canvas = transform.parent.GetChild(3).gameObject;
        buildMenu = canvas.transform.GetChild(0).gameObject;
        GameObject defensiveBuildingTab = buildMenu.transform.GetChild(0).gameObject;
        defensiveBuildingList = defensiveBuildingTab.transform.GetChild(1).gameObject;
        defensiveBuildingButton = defensiveBuildingTab.transform.GetChild(0).gameObject;
        GameObject economyBuildingTab = buildMenu.transform.GetChild(1).gameObject;
        economyBuildingList = economyBuildingTab.transform.GetChild(1).gameObject;
        economyBuildingButton = economyBuildingTab.transform.GetChild(0).gameObject;
        demolishButton = buildMenu.transform.GetChild(2).gameObject;

        playerHUD = canvas.transform.GetChild(1).gameObject;
        currencyBar = playerHUD.transform.GetChild(2).gameObject;

        GameObject settingHUD = canvas.transform.GetChild(2).gameObject;
        settingButton = settingHUD.transform.GetChild(0).gameObject;
        quitButton = settingHUD.transform.GetChild(1).gameObject;
    }

    private void CreateBuildingButtons()
    {
        // Create the building buttons
        for (int i = 0; i < buildingPrefabs.Length; i++)
        {
            GameObject parent;

            switch (buildingPrefabs[i].GetComponent<Building>().Type)
            {
                case 1:
                    parent = economyBuildingList;
                    break;
                default:
                    parent = defensiveBuildingList;
                    break;
            }

            GameObject buildingButton = Instantiate(buildingButtonPrefab, parent.transform);
            buildingButton.GetComponent<Image>().sprite = buildingPrefabs[i].GetComponent<SpriteRenderer>().sprite;

            // Create a temporary variable to capture the current value of i
            int index = i;
            buildingButton.GetComponent<Button>().onClick.AddListener(delegate { CreateBuildingGhost(buildingPrefabs[index]); });

            buildingButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = buildingPrefabs[i].GetComponent<Building>().TileName;
            buildingButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = buildingPrefabs[i].GetComponent<Building>().Price.ToString();
            buildingButtons[i] = buildingButton;
        }
    }

    private void AddListenersToSettingButtons()
    {
        // Add listener to the setting buttons
        settingButton.GetComponent<Button>().onClick.AddListener(ToggleSetting);
        quitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
        defensiveBuildingButton.GetComponent<Button>().onClick.AddListener(EnableDefensiveTab);
        economyBuildingButton.GetComponent<Button>().onClick.AddListener(EnableEconomyTab);
        demolishButton.GetComponent<Button>().onClick.AddListener(CreateDemolishGhost);
    }

    private void ToggleBuildMenu()
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
    }

    private void UpdateCurrencyBar()
    {
        currencyBar.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerScript.Currency.ToString();
    }

    public void CreateBuildingGhost(GameObject buildingPrefab)
    {
        if (buildingControlScript.GetBuildingGhost() != null)
        {
            buildingControlScript.DestroyBuildingGhost();

            if (buildingPrefab.Equals(buildingControlScript.GetBuildingGhost().GetComponent<BuildingGhost>().GetBuildingPrefab())) return;
        }

        buildingControlScript.C_DemolishMode = false;

        buildingControlScript.SetBuildingGhost(Instantiate(buildingGhostPrefab, buildingControlScript.GetBuildingGhostPos(), Quaternion.identity));
        BuildingGhost buildingGhostScript = buildingControlScript.GetBuildingGhost().GetComponent<BuildingGhost>();
        buildingGhostScript.InitSpriteRenderer();
        buildingGhostScript.SetBuilding(buildingPrefab);
    }

    public void CreateDemolishGhost()
    {
        if (buildingControlScript.GetBuildingGhost() != null)
        {
            buildingControlScript.DestroyBuildingGhost();

            if (buildingControlScript.C_DemolishMode) return;
        }

        buildingControlScript.C_DemolishMode = true;

        buildingControlScript.SetBuildingGhost(Instantiate(buildingGhostPrefab, buildingControlScript.GetBuildingGhostPos(), Quaternion.identity));
        BuildingGhost buildingGhostScript = buildingControlScript.GetBuildingGhost().GetComponent<BuildingGhost>();
        buildingGhostScript.InitSpriteRenderer();
        buildingGhostScript.SetDemolish();
    }

    public void ToggleSetting()
    {
        isSetting = !isSetting;
        quitButton.SetActive(isSetting);
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

    public void EnableDefensiveTab()
    {
        defensiveBuildingList.SetActive(true);
        DisableOtherLists(defensiveBuildingList);
    }

    public void EnableEconomyTab()
    {
        economyBuildingList.SetActive(true);
        DisableOtherLists(economyBuildingList);
    }

    private void DisableOtherLists(GameObject thisList)
    {
        if (!defensiveBuildingList.Equals(thisList))
        {
            defensiveBuildingList.SetActive(false);
        }

        if (!economyBuildingList.Equals(thisList))
        {
            economyBuildingList.SetActive(false);
        }
    }
}

