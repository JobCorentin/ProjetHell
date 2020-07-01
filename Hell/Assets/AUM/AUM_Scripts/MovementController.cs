using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public static MovementController mC;

    public Rigidbody2D rb;

    public float speed;
    public float jumpForce;

    public float antiProjectedMultiplier;

    public LayerMask groundLayers;

    float horizontalInput;
    Vector2 directionInput;

    int jumpNumb = 1;
    [HideInInspector] public bool isGrounded;

    public bool stuned = false;
    public bool projected = false;

    float timeBeforeProjectedStop;

    void Start()
    {
        mC = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rb.velocity.magnitude <= 2f)
        {
            projected = false;
        }

        isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), new Vector2(transform.position.x + 0.5f, transform.position.y - 0.51f), groundLayers);

        if(isGrounded)
        {
            jumpNumb = 1;
        }
        else
        {
            jumpNumb = 0;
        }

        if(stuned == false)
        {
            if(projected == false)
                rb.velocity = new Vector2(InputListener.iL.horizontalInput * speed * Time.fixedDeltaTime, rb.velocity.y);
            else
                rb.AddForce(new Vector2(InputListener.iL.horizontalInput * speed * Time.fixedDeltaTime * antiProjectedMultiplier * timeBeforeProjectedStop, 0), ForceMode2D.Force);

            if (InputListener.iL.jumpInput && jumpNumb > 0)
            {
                rb.velocity = Vector2.up * jumpForce * Time.fixedDeltaTime;
                jumpNumb -= 1;
            }

        }

        InputListener.iL.jumpInput = false;
    }

    public IEnumerator ProjectedFor(float time)
    {
        projected = true;

        for(float i = time; i >= 0; i -= Time.fixedDeltaTime)
        {
            timeBeforeProjectedStop = i;

            yield return new WaitForFixedUpdate();
        }

        projected = false;
    }
}
