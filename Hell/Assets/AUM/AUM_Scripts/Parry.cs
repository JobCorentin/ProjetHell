using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public Collider2D normalCol;
    public Collider2D protectionCol;

    public float protectionDuration;
    public float recoveryDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(InputListener.iL.parryInput == true)
        {

        }

        InputListener.iL.parryInput = false;
    }

    IEnumerator ActivateParry()
    {
        normalCol.gameObject.SetActive(false);
        protectionCol.gameObject.SetActive(true);

        yield break;

    }
}
