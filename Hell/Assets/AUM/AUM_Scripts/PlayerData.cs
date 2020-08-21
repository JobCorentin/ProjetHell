using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    static public PlayerData pd;

    public Vector3 position;

    public bool changePosition;

    private void Start()
    {
        pd = this;
        changePosition = false;
    }
}
