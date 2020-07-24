using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezTimeManager : MonoBehaviour
{
    public static FreezTimeManager ftm;

    bool isFrozen = false;

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
        if(isFrozen == false)
        {
            isFrozen = true;
            Time.timeScale = newScale;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1;
            isFrozen = false;
        }
    }
}
