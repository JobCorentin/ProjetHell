using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public static MovementController mC;

    public Animator animator;

    public Rigidbody2D rb;

    public float speed;
    public float jumpForce;

    float originalSpeed;

    public float antiProjectedMultiplier;

    public LayerMask groundLayers;

    float horizontalInput;
    Vector2 directionInput;

    bool canJump;
    [HideInInspector] public bool canDoubleJump;

    bool wasGrounded;

    [HideInInspector] public bool isGrounded;

    public bool stuned = false;

    float timeBeforeProjectedStop;

    public Coroutine lastChangeSpeed;

    void Start()
    {
        originalSpeed = speed;

        mC = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), new Vector2(transform.position.x + 0.5f, transform.position.y - 0.76f), groundLayers);

        if(isGrounded)
        {
            canJump = true;
            canDoubleJump = true;
        }
        else
        {
            canJump = false;
        }

        if(canDoubleJump)
        {
            BaseSlashInstancier.bsi.sr.color = new Color(BaseSlashInstancier.bsi.sr.color.r, BaseSlashInstancier.bsi.sr.color.g, BaseSlashInstancier.bsi.sr.color.b, 1f);
        }
        else
        {
            BaseSlashInstancier.bsi.sr.color = new Color(BaseSlashInstancier.bsi.sr.color.r, BaseSlashInstancier.bsi.sr.color.g, BaseSlashInstancier.bsi.sr.color.b, 0.5f);
        }

        if(stuned == false)
        {
            rb.AddForce( new Vector2(InputListener.iL.horizontalInput * speed * Time.fixedDeltaTime, 0), ForceMode2D.Force);

            if (InputListener.iL.jumpInput && (canJump == true || canDoubleJump == true))
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);

                rb.AddForce(Vector2.up * jumpForce * Time.fixedDeltaTime,ForceMode2D.Impulse);

                if (isGrounded)
                    canJump = false;
                else
                    canDoubleJump = false;
            }

        }
        animator.SetFloat("HorizontalInput", Mathf.Abs(InputListener.iL.horizontalInput));

        if (wasGrounded != isGrounded)
        {
            if(isGrounded == true)
            {
                animator.SetTrigger("AirToGround");
            }
          
            else
            {
                animator.SetTrigger("GroundToAir");
            }
                
        
        }

        animator.SetBool("JumpInput", InputListener.iL.jumpInput);

        InputListener.iL.jumpInput = false;
       
    }

    public IEnumerator ChangeSpeed(float duration, float newValue)
    {
        speed = newValue;

        for (float i = duration; i >= 0; i -= Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
        }

        speed = originalSpeed;
    }

    public void StopLastChangeSpeed()
    {
        if (lastChangeSpeed != null)
        {
            StopCoroutine(lastChangeSpeed);

            speed = originalSpeed;
        }
    }

    /*public IEnumerator ProjectedFor(float time)
    {
        projected = true;

        for(float i = time; i >= 0; i -= Time.fixedDeltaTime)
        {
            timeBeforeProjectedStop = i;

            yield return new WaitForFixedUpdate();
        }

        projected = false;
    }*/

    public IEnumerator MiniDash(Vector2 dashDirection, Rigidbody2D rigidbody2D, float duration, float movementForce, float momentumMultiplier)
    {
        stuned = true;

        for (float i = duration + momentumMultiplier; i >= momentumMultiplier; i -= Time.fixedDeltaTime)
        {
            rigidbody2D.velocity = dashDirection.normalized * movementForce * i * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        stuned = false;
    }

}
