using Mirror;
using TMPro;
using UnityEngine;

public class GameNetworkManager : MonoBehaviour
{
    NetworkManager networkManager;
    string ip;
    public GameObject ipInputFieldObj;
    public TextMeshProUGUI warningText;

    void Start()
    {
        networkManager = GetComponent<NetworkManager>();
    }
    public void StartSinglePlayer()
    {
        networkManager.maxConnections = 0; 
        networkManager.StartHost();
    }
    public void StartHost()
    {
        networkManager.StartHost();
    }

    public void JoinGame()
    {
        if (string.IsNullOrEmpty(ip))
        {
            warningText.text = "Please enter the correct host IP!";
            return;
        }

        networkManager.networkAddress = ip;
        networkManager.StartClient();
    }

    public void SetIP(string ip)
    {
        this.ip = ip;
    }
}
