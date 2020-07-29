using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude < 0.1f)
            return;

        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            //gameObject.transform.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            //gameObject.transform.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
