using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiOneController : EnnemiController
{
    public float coolDown;
    float coolDownTimer = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if(stunned == true)
        {
            return;
        }

        target = pTransform.position + ((transform.position - pTransform.position).normalized * range);

        if (Vector2.Distance(target, transform.position) >= 0.5f)
        {
            base.FixedUpdate();
        }
        
        if(Vector2.Distance(transform.position, pTransform.position) <= range + 1f)
        {
            if(coolDownTimer < coolDown)
            {
                coolDownTimer += Time.fixedDeltaTime;
            }
            else
            {
                Debug.Log("Attaque");
                coolDownTimer = 0;
            }
                


        }

        //if(Vector2.Distance(transform.position, pTransform.position))
        //rb.AddForce(pTransform)
    }
}
