using Mirror;
using TMPro;
using UnityEngine;

public class FOTUNetworkManager : NetworkManager
{
    public GameObject ipInputFieldObj;
    public TextMeshProUGUI warningText;
    public GameObject[] enemyPrefabs;
    public GameObject[] buildingPrefabs;
    public GameObject[] resourcesPrefab;
    public GameObject[] projectilesPrefab;

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

        foreach (GameObject resource in resourcesPrefab)
        {
            spawnPrefabs.Add(resource);
        }

        foreach (GameObject projectile in projectilesPrefab)
        {
            spawnPrefabs.Add(projectile);
        }
    }

    public void OnSingleplayerButtonClick()
    {
        maxConnections = 0; 
        StartHost();
    }
    public void OnHostButtonClick()
    {
        StartHost();
    }

    public void OnJoinButtonClick()
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
