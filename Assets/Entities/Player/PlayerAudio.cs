using Mirror;
using UnityEngine;

public class PlayerAudio : NetworkBehaviour
{
    private AudioListener audioListener;

    void Start()
    {
        audioListener = GetComponentInChildren<AudioListener>();

        if (!isLocalPlayer)
            audioListener.enabled = false;
        else
            audioListener.enabled = true;
    }
}
