using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameNetworkManager : MonoBehaviour
{
    NetworkManager networkManager;
    InputField ipInputField;
    public GameObject ipInputFieldObj;
    public TextMeshProUGUI warningText;

    void Start()
    {
        networkManager = GetComponent<NetworkManager>();
        ipInputField = ipInputFieldObj.GetComponent<InputField>();
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
        if (ipInputField == null || string.IsNullOrEmpty(ipInputField.text))
        {
            warningText.text = "Please enter the correct host IP!";
            return;
        }

        networkManager.networkAddress = ipInputField.text;
        networkManager.StartClient();
    }
}
