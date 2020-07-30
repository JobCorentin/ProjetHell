﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlashInstancier : MonoBehaviour
{
    public Animator animator;

    public static BaseSlashInstancier bsi;

    public GameObject slash;

    public GameObject bloodSlash;

    public bool bloodEveryDirection = false;

    public float movementForce;
    public float airMovementForce;

    public SpriteRenderer sr;

    [HideInInspector] public bool bloodMode;

    public float coolDown;
    public float bloodCoolDown;

    float coolDownTimer;

    public float duration;

    public float momentumMultiplier;

    public float bloodDuration;

    Coroutine lastSlash;
    Coroutine lastStarting;

    public float currentAutoAimAngle;
    public int currentNumberOfRaycast;

    [HideInInspector] public bool canGainHeight;
    [HideInInspector] public bool canBounce = false;

    public int slashNumb;
    [HideInInspector] public int slashNumbMax;

    [HideInInspector]public float attackDirectionAngle;


    void Start()
    {
        bsi = this;

        slashNumbMax = slashNumb;

        slash.SetActive(false);
        bloodSlash.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(bloodMode == true)
        {
            sr.color = new Color(Color.red.r, Color.red.g, Color.red.b, sr.color.a);

            if (coolDownTimer < bloodCoolDown)
            {
                coolDownTimer += Time.fixedDeltaTime;
            }

            if (InputListener.iL.bloodModeInput == true && coolDownTimer > bloodCoolDown / 2f && slashNumb > 0)
            {
                //if (BloodManager.bm.bloodNumb >= 3)
                    //AttackDirectionDecision();
            }
        }
        else
        {
            sr.color = new Color(Color.white.r, Color.white.g, Color.white.b, sr.color.a);

            if (coolDownTimer < coolDown)
            {
                coolDownTimer += Time.fixedDeltaTime;
            }

            if (InputListener.iL.attackInput == true && coolDownTimer > coolDown / 2f /*&& slashNumb > 0*/)
            {
                AttackDirectionDecision(false);
            }

            if (InputListener.iL.bloodModeInput == true && coolDownTimer > bloodCoolDown / 2f && slashNumb > 0)
            {
                if (BloodManager.bm.bloodNumb >= 3)
                    AttackDirectionDecision(true);
            }

        }

        /*if(InputListener.iL.bloodModeInput == true)
        {
            bloodMode = !bloodMode;
        }*/

        InputListener.iL.bloodModeInput = false;
        InputListener.iL.attackInput = false;
    }

    void AttackDirectionDecision(bool bloodAttack)
    {
        Vector2 currentAttackDirection = InputListener.iL.directionVector.normalized;
        Vector2 currentInputDirection = InputListener.iL.directionVector.normalized;

        float attackDirectionAngle = Vector2.Angle(transform.right, currentAttackDirection);

        if (currentAttackDirection.y < 0)
        {
            attackDirectionAngle = -attackDirectionAngle;
        }

        if (MovementController.mC.isGrounded == true)
        {
            if (attackDirectionAngle < 45 && -90 < attackDirectionAngle)
                currentAttackDirection = Vector2.right;
            else if (45 <= attackDirectionAngle && attackDirectionAngle <= 135)
                currentAttackDirection = Vector2.up;
            else if (attackDirectionAngle < 135 || -90 < attackDirectionAngle)
                currentAttackDirection = Vector2.left;
        }
        else
        {
            if (attackDirectionAngle < 45 && -45 < attackDirectionAngle)
                currentAttackDirection = Vector2.right;
            else if (45 <= attackDirectionAngle && attackDirectionAngle <= 135)
                currentAttackDirection = Vector2.up;
            else if (attackDirectionAngle > 135 || -135 > attackDirectionAngle)
                currentAttackDirection = Vector2.left;
            else if (-135 <= attackDirectionAngle && attackDirectionAngle <= -45)
                currentAttackDirection = Vector2.down;
        }

        if (lastStarting == null)
            lastStarting = StartCoroutine(StartSlash(bloodAttack ,currentAttackDirection, currentInputDirection));
    }

    IEnumerator StartSlash(bool bloodAttack, Vector2 currentAttackDirection, Vector2 currentInputDirection)
    {
        while (lastSlash != null || coolDownTimer < coolDown)
        {
            yield return null;
        }

        if (bloodAttack == true)
        {
            lastSlash = StartCoroutine(BloodSlash(currentAttackDirection, currentInputDirection));
        }
        else
        {
            if(MovementController.mC.isWallSliding == true)
            {
                lastSlash = StartCoroutine(Slash(new Vector2(-currentAttackDirection.x, currentAttackDirection.y), new Vector2(-currentInputDirection.x, currentInputDirection.y)));
            }
            else
            {
                lastSlash = StartCoroutine(Slash(currentAttackDirection, currentInputDirection));
            }

            
        }

        lastStarting = null;
    }

    IEnumerator Slash(Vector2 currentAttackDirection, Vector2 currentInputDirection)
    {
        animator.SetTrigger("Attacking");

        StartCoroutine(IsAttackingFor());

        slash.SetActive(true);
        BaseSlashCollision.bsc.ennemiTouched = false;

        Parry.p.protectionCol.gameObject.SetActive(true);

        if (currentAttackDirection == Vector2.down)
            canBounce = true;


        attackDirectionAngle = Vector2.Angle(transform.right, currentAttackDirection);

        if (currentAttackDirection.y < 0)
        {
            attackDirectionAngle = -attackDirectionAngle;
        }

        animator.SetFloat("AttackAngle", attackDirectionAngle);

        //slash.transform.rotation = Quaternion.Euler(0, 0, attackDirectionAngle);

        MovementController.mC.stuned = true;

        for (float i = duration + momentumMultiplier; i >= momentumMultiplier; i -= Time.fixedDeltaTime)
        {
            if(slashNumb > 0 && BaseSlashCollision.bsc.bouncing == false)
            {
                //ennemiController.rb.velocity = (currentAttackDirection.normalized * 1.5f + currentInputDirection.normalized).normalized * movementForce * 1.3f * i * Time.fixedDeltaTime;
                //if(currentAttackDirection == Vector2.up)
                if (MovementController.mC.isGrounded == true)
                {
                    if (canGainHeight == true || currentInputDirection.y < 0)
                        MovementController.mC.rb.velocity = (currentAttackDirection.normalized + currentInputDirection.normalized).normalized * movementForce * i * Time.fixedDeltaTime;
                    else
                        MovementController.mC.rb.velocity = new Vector2(((currentAttackDirection.normalized + currentInputDirection.normalized).normalized.x * movementForce * i * Time.fixedDeltaTime) * 3f / 4f, ((currentAttackDirection.normalized + currentInputDirection.normalized).normalized.y * movementForce * i * Time.fixedDeltaTime) / 2);
                }
                else if (MovementController.mC.isGrounded == false)
                {
                    if (canGainHeight == true || currentInputDirection.y < 0)
                        MovementController.mC.rb.velocity = (currentAttackDirection.normalized + currentInputDirection.normalized).normalized * airMovementForce * i * Time.fixedDeltaTime;
                    else
                        MovementController.mC.rb.velocity = new Vector2(((currentAttackDirection.normalized + currentInputDirection.normalized).normalized.x * airMovementForce * i * Time.fixedDeltaTime) * 3f / 4f, ((currentAttackDirection.normalized + currentInputDirection.normalized).normalized.y * airMovementForce * i * Time.fixedDeltaTime) / 2);
                }


                //}
                //else
                //{
                //    MovementController.mC.rb.velocity = (currentAttackDirection.normalized + currentInputDirection.normalized).normalized * movementForce * i * Time.fixedDeltaTime;
                //}
            }

            yield return new WaitForFixedUpdate();
        }

        //if (currentAttackDirection == Vector2.up)
        //{
        if(BaseSlashCollision.bsc.ennemiTouched == false)
        {
            canGainHeight = false;
            slashNumb--;
        }
        //}

        MovementController.mC.stuned = false;

        if (slashNumb > 0)
        {
            BetterJump.bj.StopLastChangeFall();

            BetterJump.bj.lastChangeFall = BetterJump.bj.StartCoroutine(BetterJump.bj.ChangeFallMultiplier(0.1f, BetterJump.bj.fallMultiplier / 10f));


            MovementController.mC.StopLastChangeSpeed();

            MovementController.mC.lastChangeSpeed = MovementController.mC.StartCoroutine(MovementController.mC.ChangeSpeed(0.1f, MovementController.mC.speed / 5f));
        }

        /*for (float i = duration + momentumMultiplier; i >= momentumMultiplier; i -= Time.fixedDeltaTime)
        {
            //MovementController.mC.rb.velocity = currentInputDirection * movementForce * i * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }*/

            canBounce = false;

        Parry.p.protectionCol.gameObject.SetActive(false);

        slash.SetActive(false);

        coolDownTimer = 0;
        lastSlash = null;
    }

    IEnumerator IsAttackingFor()
    {
        animator.SetBool("IsAttacking", true);

        yield return new WaitForSeconds(duration);

        animator.ResetTrigger("GroundToAir");

        animator.SetBool("IsAttacking", false);
    }
    IEnumerator BloodSlash(Vector2 currentAttackDirection, Vector2 currentInputDirection)
    {
        bloodSlash.SetActive(true);
        bloodSlash.transform.GetChild(0).GetComponent<Animator>().SetTrigger("bloodAttack");

        BloodManager.bm.bloodNumb -= 3;

        GameObject element = null;
        Transform selectedElement = null;
        float minimumAngle = 100;
        float subAngle = currentAutoAimAngle / currentNumberOfRaycast;
        float startAngle = Vector2.SignedAngle(Vector2.right, InputListener.iL.directionVector) - currentAutoAimAngle / 2;
        Vector2 raycastDirection;
        Vector2 autoAimDirection = InputListener.iL.directionVector;

        for (int i = 0; i < currentNumberOfRaycast; i++)
        {
            float angle = startAngle + i * subAngle;
            raycastDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, 10, LayerMask.GetMask("Ennemi"));
            if (hit)
            {
                if(hit.collider.gameObject.tag == "Ennemi")
                    element = hit.collider.gameObject;

                if (element != null && minimumAngle > Mathf.Abs(Vector2.SignedAngle(Vector2.right, InputListener.iL.directionVector) - angle))
                {
                    minimumAngle = startAngle + i * subAngle;
                    selectedElement = element.transform;
                }
            }
        }
        if (selectedElement != null)
        {
            autoAimDirection = selectedElement.transform.position - transform.position;
        }

        float attackDirectionAngle = Vector2.Angle(transform.right, currentAttackDirection);

        if (bloodEveryDirection)
        {
            attackDirectionAngle = Vector2.Angle(transform.right, autoAimDirection);
        }
        else if (!bloodEveryDirection)
        {
            attackDirectionAngle = Vector2.Angle(transform.right, currentAttackDirection);
        }

        if (autoAimDirection.y < 0)
        {
            attackDirectionAngle = -attackDirectionAngle;
        }

        bloodSlash.transform.rotation = Quaternion.Euler(0, 0, attackDirectionAngle);

        for (float i = duration + momentumMultiplier; i >= momentumMultiplier; i -= Time.fixedDeltaTime)
        {
            //ennemiController.rb.velocity = (currentAttackDirection.normalized * 1.5f + currentInputDirection.normalized).normalized * movementForce * 1.3f * i * Time.fixedDeltaTime;
            //if (currentAttackDirection == Vector2.up)
            //{
                if (canGainHeight == true)
                    MovementController.mC.rb.velocity = (currentAttackDirection.normalized + currentInputDirection.normalized).normalized * movementForce * i * Time.fixedDeltaTime;
                else
                    MovementController.mC.rb.velocity = new Vector2((currentAttackDirection.normalized + currentInputDirection.normalized).normalized.x * movementForce * i * Time.fixedDeltaTime, ((currentAttackDirection.normalized + currentInputDirection.normalized).normalized.y * movementForce * i * Time.fixedDeltaTime) / 4);
            //}
            //else
            //{
                //MovementController.mC.rb.velocity = (currentAttackDirection.normalized + currentInputDirection.normalized).normalized * movementForce * i * Time.fixedDeltaTime;
            //}

            yield return new WaitForFixedUpdate();
        }

        //if (currentAttackDirection == Vector2.up)
        //{
        canGainHeight = false;
        slashNumb--;
        //}

        BetterJump.bj.StopLastChangeFall();

        BetterJump.bj.lastChangeFall = BetterJump.bj.StartCoroutine(BetterJump.bj.ChangeFallMultiplier(0.3f, BetterJump.bj.fallMultiplier / 10f));


        MovementController.mC.StopLastChangeSpeed();

        MovementController.mC.lastChangeSpeed = MovementController.mC.StartCoroutine(MovementController.mC.ChangeSpeed(0.3f, MovementController.mC.speed / 5f));
        bloodSlash.transform.GetChild(1).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        bloodSlash.SetActive(false);

        coolDownTimer = 0;
        lastSlash = null;
    }

    public void ResetSlashNumb()
    {
        slashNumb = slashNumbMax;
    }
}
