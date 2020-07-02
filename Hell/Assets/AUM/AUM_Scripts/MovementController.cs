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

    bool canJump;
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
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        if(stuned == false)
        {
            if(projected == false)
                rb.AddForce( new Vector2(InputListener.iL.horizontalInput * speed * Time.fixedDeltaTime, 0));
            else
                rb.AddForce(new Vector2(InputListener.iL.horizontalInput * speed * Time.fixedDeltaTime * antiProjectedMultiplier * timeBeforeProjectedStop, 0), ForceMode2D.Force);

            if (InputListener.iL.jumpInput && canJump == true)
            {
                rb.AddForce(Vector2.up * jumpForce * Time.fixedDeltaTime,ForceMode2D.Impulse);
                canJump = false;
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
