using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadOptionData : MonoBehaviour
{
    public static DontDestroyOnLoadOptionData ddolod;

    private void Awake()
    {
        if (!ddolod)
        {
            ddolod = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
