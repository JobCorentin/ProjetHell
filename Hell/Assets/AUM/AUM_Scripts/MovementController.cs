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
    bool wasWalled;
    bool wasWallSliding;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isWalled;
    [HideInInspector] public bool isWallSliding;

    float horizontalInputWallSliding;

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
        wasGrounded = false;
        isGrounded = false;

        originalSpeed = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        wasGrounded = isGrounded;

        wasWalled = isWalled;

        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.5f, 0.2f), 0, groundLayers);

        isWalled = Physics2D.OverlapBox(leftWallCheck.position, new Vector2(0.2f, 0.75f), 0, wallLayers);

        if(isWalled == false)
        {
            isWalled = Physics2D.OverlapBox(rightWallCheck.position, new Vector2(0.2f, 0.75f), 0, wallLayers);
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



        wasWallSliding = isWallSliding;

        if (isWalled && isGrounded == false && Mathf.Abs( InputListener.iL.horizontalInput ) > 0) 
        {
            isWallSliding = true;
            canDoubleJump = true;

            //if(was)

        }
        else
        {
            isWallSliding = false;
        }

        if( isWallSliding )
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -speedWallSlide, float.MaxValue));
        }

        if(stuned == false)
        {
            rb.AddForce( new Vector2(InputListener.iL.horizontalInput * speed * Time.fixedDeltaTime, 0), ForceMode2D.Force);

            if(isWallSliding)
            {
                if (InputListener.iL.jumpInput)
                {
                    rb.AddForce(new Vector2(xWallJump * -InputListener.iL.horizontalInput, yWallJump).normalized * wallJumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);

                    animator.SetTrigger("Jumping");

                    StartCoroutine(IsJumpingFor());

                    // animator.GetCurrentAnimatorClipInfo(0).Length;
                }
                    
                //StartCoroutine(MiniDash(new Vector2(xWallJumpForce * -InputListener.iL.horizontalInput, yWallJumpForce).normalized, rb, wallJumpTime, new Vector2(xWallJumpForce, yWallJumpForce).magnitude, 1)) ;
            }
            else
            {
                if (InputListener.iL.jumpInput && (canJump == true || canDoubleJump == true))
                {
                    if(canJump == true)
                    {
                        animator.SetTrigger("Jumping");
                        if(canDoubleJump == true)
                        {
                            FXManager.fxm.fxInstancier(0, groundCheck);
                        }

                        StartCoroutine(IsJumpingFor());
                    }
                    else if(canDoubleJump == true)
                    {
                        animator.SetTrigger("DoubleJumping");
                    }

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
        animator.SetFloat("VerticalInput", InputListener.iL.verticalInput);

        animator.SetBool("IsGrounded", isGrounded);


        if (wasGrounded != isGrounded)
        {
            if(isGrounded == true)
            {
                animator.SetTrigger("AirToGround");
                FXManager.fxm.fxInstancier(1, groundCheck);
            }
            else
            {
                animator.SetTrigger("GroundToAir");
            }
        }

        if (wasWalled != isWalled)
        {
            if (isWalled == true)
            {
                animator.SetTrigger("AirToWall");
            }
            else
            {
                animator.SetTrigger("WallToAir");
            }
        }

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

    IEnumerator IsJumpingFor()
    {
        animator.SetBool("IsJumping", true);

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("IsJumping", false);
    }

    public void DustInstancier(GameObject dust,int i)
    {
        //Dust Jump
        if(i == 0)
        {
            if(canDoubleJump == true)
            {
                Instantiate(dust, groundCheck.transform.position, Quaternion.identity);
            }
        }
        //Dust Land
        if (i == 1)
        {
            Instantiate(dust, groundCheck.transform.position, Quaternion.identity);
        }
    }
}
