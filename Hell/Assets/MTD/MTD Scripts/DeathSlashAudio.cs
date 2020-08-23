using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSlashAudio : MonoBehaviour
{
    public AK.Wwise.Event deathSlashAudio;

    // Start is called before the first frame update
    void OnEnable()
    {
        deathSlashAudio.Post(gameObject);
    }
}
