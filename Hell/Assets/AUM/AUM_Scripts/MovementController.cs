﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public static MovementController mC;

    public Animator animator;

    public Rigidbody2D rb;

    public Collider2D col;

    public float speed;
    public float jumpForce;
    public float secondJumpForce;

    float originalSpeed;
    [HideInInspector] public float speedMultiplier = 1;

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

    Vector2 wallSlideBaseInputDir;

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

    [HideInInspector] public bool pushedBack;

    float isGroundedFor = 0;

    public ParticleSystem psFireHair;
    public ParticleSystem psSmallFireHair;
    public ParticleSystem psBurstFireHair;

    bool couldDoubleJump = true;

    [Space(5)]
    [Header("Sound")]
    public AK.Wwise.Event playerJumpAudio;


    private void Awake()
    {
        mC = this;
    }

    void Start()
    {
        wasGrounded = false;
        isGrounded = false;
        pushedBack = false;
        originalSpeed = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        wasGrounded = isGrounded;

        wasWalled = isWalled;

        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.5f, 0.2f), 0, groundLayers);

        isWalled = Physics2D.OverlapBox(leftWallCheck.position, new Vector2(0.1f, 0.75f), 0, wallLayers);

        if(isWalled == false)
        {
            isWalled = Physics2D.OverlapBox(rightWallCheck.position, new Vector2(0.1f, 0.75f), 0, wallLayers);
        }

        if (isGrounded)
        {
            canJump = true;
            canDoubleJump = true;
            BaseSlashInstancier.bsi.canGainHeight = true;
            BaseSlashInstancier.bsi.ResetSlashNumb();
        }
        else
        {
            canJump = false;
        }

        if (isGrounded)
        {
            isGroundedFor += Time.fixedDeltaTime;
        }
        else
        {
            isGroundedFor = 0;
        }

        wasWallSliding = isWallSliding;

        if (isWalled && isGrounded == false) 
        {
            if(isWallSliding == false)
            {
                if (Physics2D.OverlapBox(leftWallCheck.position, new Vector2(0.2f, 0.75f), 0, wallLayers))
                    wallSlideBaseInputDir = Vector2.left;
                else if (Physics2D.OverlapBox(rightWallCheck.position, new Vector2(0.2f, 0.75f), 0, wallLayers))
                    wallSlideBaseInputDir = Vector2.right;
            }

            isWallSliding = true;
            canDoubleJump = true;
            BaseSlashInstancier.bsi.ResetSlashNumb();

            //if(was)

        }
        else
        {
            isWallSliding = false;
        }

        if( isWallSliding )
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, ( (-speedWallSlide / (Mathf.Abs(InputListener.iL.horizontalInput))) * 0.5f ) + ( -speedWallSlide * 0.5f), float.MaxValue));
        }

        if(canDoubleJump)
        {
            if(couldDoubleJump == false)
            {
                psBurstFireHair.Play();

                psFireHair.Play();
                psSmallFireHair.Play();
                psFireHair.loop = true;
                psSmallFireHair.loop = true;
            }
        }
        else
        {
            psFireHair.loop = false;
            psSmallFireHair.loop = false;
        }

        if(stuned == false)
        {
            if (isGrounded)
                rb.velocity = new Vector2(Mathf.Round(InputListener.iL.horizontalInput) * speed * speedMultiplier * Time.fixedDeltaTime / 7, 0);
            else
                rb.AddForce( new Vector2(InputListener.iL.horizontalInput * speed * speedMultiplier * Time.fixedDeltaTime, 0), ForceMode2D.Force);

            //Empêche le PJ de glisser
            if(InputListener.iL.horizontalInput == 0 && isGrounded)
            {
                rb.velocity = new Vector2(0,rb.velocity.y);
            }

            if(isWallSliding)
            {
                if (InputListener.iL.jumpInput)
                {
                    rb.AddForce(new Vector2(xWallJump * -wallSlideBaseInputDir.x, yWallJump).normalized * wallJumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);

                    animator.SetTrigger("Jumping");
                    playerJumpAudio.Post(gameObject);

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
                        playerJumpAudio.Post(gameObject);

                        if (canDoubleJump == true)
                        {
                            FXManager.fxm.fxInstancier(0, groundCheck,0);
                        }

                        //StartCoroutine(IsJumpingFor());
                    }
                    else if(canDoubleJump == true)
                    {
                        animator.SetTrigger("DoubleJumping");
                        playerJumpAudio.Post(gameObject);

                        FXManager.fxm.fxInstancier(5, groundCheck, 0);
                    }

                    rb.velocity = new Vector2(rb.velocity.x, 0);

                    if(canJump ==true)
                        rb.AddForce(Vector2.up * jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
                    else if (canJump ==false)
                        rb.AddForce(Vector2.up * secondJumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);


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
                FXManager.fxm.fxInstancier(1, groundCheck,0);
            }
            else
            {
                animator.SetTrigger("GroundToAir");
            }
        }
        else if(isGroundedFor > 0.5f)
        {
            animator.ResetTrigger("AirToGround");
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

        couldDoubleJump = canDoubleJump;

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
        //pushedBack = false;
        stuned = false;
    }

    IEnumerator IsJumpingFor()
    {
        animator.SetBool("IsJumping", true);

        yield return new WaitForSeconds(0.15f);

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

    public IEnumerator Pushed(float duration)
    {
        pushedBack = true;
        yield return new WaitForSeconds(duration);
        pushedBack = false;
    }
}
