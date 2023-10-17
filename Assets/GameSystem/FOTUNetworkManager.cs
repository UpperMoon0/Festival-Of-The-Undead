using Mirror;
using TMPro;
using UnityEngine;

public class FOTUNetworkManager : NetworkManager
{
    public GameObject ipInputFieldObj;
    public TextMeshProUGUI warningText;
    public GameObject[] enemyPrefabs = new GameObject[1];
    public GameObject[] buildingPrefabs = new GameObject[2];

    private string ip;

    public override void Awake()
    {
        foreach (GameObject enemy in enemyPrefabs)
        {
            spawnPrefabs.Add(enemy);
        }

        foreach (GameObject building in buildingPrefabs)
        {
            spawnPrefabs.Add(building);
        }
    }

    public void StartSinglePlayer()
    {
        maxConnections = 0; 
        StartHost();
    }
    public void StartGameHost()
    {
        StartHost();
    }

    public void JoinGame()
    {
        if (string.IsNullOrEmpty(ip))
        {
            warningText.text = "Please enter the correct host IP!";
            return;
        }

        networkAddress = ip;
        StartClient();
    }

    public void SetIP(string ip)
    {
        this.ip = ip;
    }
}
