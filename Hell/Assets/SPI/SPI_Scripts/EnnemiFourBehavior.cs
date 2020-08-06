using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiFourBehavior : EnnemiController
{

    public EnnemiDetection ennemiDetection;
    public bool canAttack;
    [HideInInspector] public bool charge;
    public GameObject slash;

    [HideInInspector] public Transform targetTransform;

    [HideInInspector] public Vector2 lookAt;


    /*
    public Vector2 dashImpulsion;
    [HideInInspector] public Vector2 currentDash;
    public float duration;
    public float movementForce;
    public float momentumMultiplier;
    */
    public float preparationDuration;


    public override void Start()
    {
        lookAt = Vector2.left;
        base.Start();
        canAttack = true;
        coolDownTimer = coolDown - 0.3f;
        currentDash = dashImpulsion;
    }

    public override void FixedUpdate()
    {
        if (stunned == true)
        {
            return;
        }



        if (lookAt.x <= 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);

        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (playerDetected == false)
        {
            Detection();
        }
        else
        {
            targetTransform = pTransform;
            target = targetTransform.position;

            for (int i = 0; i < ennemiDetection.ennemiControllers.Count; i++)
            {
                target += (Vector2)(transform.position - ennemiDetection.ennemiControllers[i].transform.position).normalized * 3f; //distance ennemis entre eux
            }

            if (canAttack)
            {
                if (target.y > transform.position.y + 10) //Si le joueur est suffisamment haut, le centaure lance un katana
                {

                }
                else // Charge
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, lookAt, Mathf.Infinity, 17);
                    if (hit == true)
                    {
                        target = hit.transform.position;
                        canAttack = false;
                        StartCoroutine(PrepareCharge());
                    }
                }
            }

            if (charge)
            {
                base.FixedUpdate();
            }
            /*if (Vector2.Distance(target, transform.position) >= 0.5f && stunned == false)
            {
                base.FixedUpdate();
            }*/


            /*
            if (Vector2.Distance(transform.position, pTransform.position) <= range * 2f && canAttack)
            {
                if (coolDownTimer >= coolDown)
                {

                    if (lookAt.x >= 0)
                        currentDash.x = dashImpulsion.x;
                    else if (lookAt.x < 0)
                        currentDash.x = -dashImpulsion.x;

                    StartCoroutine(PrepareAttack());
                    canAttack = false;
                    coolDownTimer = 0;
                }
                else
                {
                    coolDownTimer += Time.fixedDeltaTime;
                }
            }
            else if (canAttack == false)
                animator.SetBool("Attack", false);
                */
        }
    }

    
    IEnumerator PrepareCharge()
    {
        animator.SetBool("IsPreparingCharge", true);
        yield return new WaitForSeconds(preparationDuration);
        slash.SetActive(true);
        charge = true;
    }
    /*
    void EndAttack()
    {
        //slash.SetActive(false);
        stunned = false;
        canAttack = true;
        animator.SetBool("Attack", false);
    }

    public void StopAttack()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("IsPreparing", false);
        stunned = true;
        //slash.SetActive(false);
        animator.SetBool("IsStun", true);
        StartCoroutine(Recover());
    }
    IEnumerator Recover()
    {
        yield return new WaitForSeconds(0.5f);
        stunned = false;
        canAttack = true;
        animator.SetBool("IsStun", false);
        //slash.GetComponent<ZombiSlashCollision>().reflected = false;
    }

    public void PushedBack()
    {
        gameObject.layer = 10;
        //slash.SetActive(false);
        animator.SetBool("Attack", false);
        animator.SetBool("IsPreparing", false);
        animator.SetBool("IsStun", true);
        stunned = true;
        Vector2 knockBack = new Vector2(-currentDash.x * 2, currentDash.y * 1.5f);
        for (float i = duration + momentumMultiplier; i >= momentumMultiplier; i -= Time.fixedDeltaTime)
        {
            rb.velocity = knockBack.normalized * movementForce * i * Time.fixedDeltaTime;

        }
        gameObject.layer = 16;
        StartCoroutine(Recover());
    }*/
}
