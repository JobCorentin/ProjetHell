using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatueBossGuard : MonoBehaviour
{
    public bool destroyed;

    void Destroyed()
    {
        destroyed = true;
    }
}
