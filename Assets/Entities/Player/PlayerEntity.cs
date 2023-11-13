using Mirror;
using UnityEngine;

public class PlayerEntity : NetworkBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject uiCamera;

    void Start()
    {
        player = transform.GetChild(0).gameObject;
        uiCamera = transform.GetChild(1).gameObject;
        playerCamera = transform.GetChild(2).gameObject;

        if (!isLocalPlayer)
        {
            playerCamera.SetActive(false);
            playerCamera.GetComponent<AudioListener>().enabled = false;
            uiCamera.gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (player != null)
            {
                playerCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
                uiCamera.transform.position = player.transform.position;
            }
        }
    }
}
