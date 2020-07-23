using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputListener.iL.horizontalInput < 0)
        {
           transform.localScale = new Vector3(-1, 1, 1);
            //gameObject.transform.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (InputListener.iL.horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            //gameObject.transform.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
