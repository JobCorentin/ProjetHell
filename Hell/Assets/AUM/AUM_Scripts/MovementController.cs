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
    public LayerMask wallLayers;

    float horizontalInput;
    Vector2 directionInput;

    bool canJump;
    [HideInInspector] public bool canDoubleJump;
    bool canWallJump;

    bool wasGrounded;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isWalled;
    [HideInInspector] public bool isWallSliding;

    public Transform groundCheck;
    public Transform leftWallCheck;
    public Transform rightWallCheck;

    public float speedWallSlide;

    public float xWallJump;
    public float yWallJump;

    public float wallJumpForce;

    public bool stuned = false;

    float timeBeforeProjectedStop;

    public Coroutine lastChangeSpeed;

    private void Awake()
    {
        mC = this;
    }

    void Start()
    {
        originalSpeed = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.5f, 0.2f), 0, groundLayers);

        isWalled = Physics2D.OverlapBox(leftWallCheck.position, new Vector2(0.2f, 0.75f), 0, wallLayers);

        if(isWalled == false)
        {
            isWalled = Physics2D.OverlapBox(rightWallCheck.position, new Vector2(0.2f, 0.75f), 0, wallLayers);
        }
        else
        {
            Debug.Log("oui");
        }

        if (isGrounded)
        {
            canJump = true;
            canDoubleJump = true;
            BaseSlashInstancier.bsi.canGainHeight = true;
            BaseSlashInstancier.bsi.slashNumb = BaseSlashInstancier.bsi.slashNumbMax;
        }
        else
        {
            canJump = false;
        }

        isWallSliding = false;

        if (isWalled && isGrounded == false && Mathf.Abs( InputListener.iL.horizontalInput ) > 0) 
        {
            isWallSliding = true;
            canDoubleJump = true;
        }

        if( isWallSliding )
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -speedWallSlide, float.MaxValue));
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

            if(isWallSliding)
            {
                if (InputListener.iL.jumpInput)
                    rb.AddForce(new Vector2(xWallJump * -InputListener.iL.horizontalInput, yWallJump).normalized * wallJumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
                //StartCoroutine(MiniDash(new Vector2(xWallJumpForce * -InputListener.iL.horizontalInput, yWallJumpForce).normalized, rb, wallJumpTime, new Vector2(xWallJumpForce, yWallJumpForce).magnitude, 1)) ;
            }
            else
            {
                if (InputListener.iL.jumpInput && (canJump == true || canDoubleJump == true))
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);

                    rb.AddForce(Vector2.up * jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);

                    if (isGrounded)
                        canJump = false;
                    else
                        canDoubleJump = false;
                }
            }

        }
        animator.SetFloat("HorizontalInput", Mathf.Abs(InputListener.iL.horizontalInput));

        animator.SetBool("IsGrounded", isGrounded);

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

        animator.SetBool("IsOnWall", isWalled);

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
