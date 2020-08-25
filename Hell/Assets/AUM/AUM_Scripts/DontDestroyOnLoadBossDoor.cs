using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadBossDoor : MonoBehaviour
{
    public static DontDestroyOnLoadBossDoor ddolbd;

    private void Awake()
    {
        if (!ddolbd)
        {
            ddolbd = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
