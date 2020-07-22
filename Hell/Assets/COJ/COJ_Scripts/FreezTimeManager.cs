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

    private void FixedUpdate()
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
            var originalTime = Time.timeScale;
            Time.timeScale = newScale;

            yield return new WaitForSecondsRealtime(duration);

            Time.timeScale = originalTime;
            isFrozen = false;
        }
    }
}
