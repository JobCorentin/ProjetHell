using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnterAudio : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Player") && SoundManager.instance.isBossFight == false)
        {
            SoundManager.instance.isBossFight = true;
        }
    }
}
