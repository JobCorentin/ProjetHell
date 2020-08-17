using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepScript : MonoBehaviour
{
    public AK.Wwise.Event playerFootstepAudio;

    // Start is called before the first frame update
    void OnEnable()
    {
        playerFootstepAudio.Post(gameObject);
    }
}
