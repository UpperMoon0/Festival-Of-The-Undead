using Mirror;
using UnityEngine;

public class LocalPlayerInit : NetworkBehaviour
{
    void Start()
    {
        if (!isLocalPlayer)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(1).GetComponent<AudioListener>().enabled = false;
            transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
