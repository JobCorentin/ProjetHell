using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePlayerForEnding : MonoBehaviour
{
    public Transform TForEnding;
    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        Player.transform.position = TForEnding.position;
    }

    private void FixedUpdate()
    {
        Player.transform.position = TForEnding.position;
    }
}
