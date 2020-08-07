using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezTimeManager : MonoBehaviour
{
    public static FreezTimeManager ftm;

    bool isFrozen = false;
    bool canFreeze = true;

    private void Awake()
    {
        ftm = this;
    }

    private void Update()
    {
        if(isFrozen == false)
        {
            Time.timeScale = 1;
        }
    }

    public IEnumerator FreezeTimeFor(float duration, float newScale)
    {
        if(isFrozen == false && canFreeze == true)
        {
            isFrozen = true;
            canFreeze = false;
            Time.timeScale = newScale;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1;
            isFrozen = false;
            canFreeze = true;
        }
    }
}
