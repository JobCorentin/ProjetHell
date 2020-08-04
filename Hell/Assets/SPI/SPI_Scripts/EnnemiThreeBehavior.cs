using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiThreeBehavior : EnnemiController
{

    public EnnemiDetection ennemiDetection;
    public GameObject slash;
    Coroutine lastSlash;
    public bool canAttack;

    [HideInInspector] public Vector2 lookAt;
    public Vector2 dashImpulsion;
    [HideInInspector] public Vector2 currentDash;


    public float duration;
    public float movementForce;
    public float momentumMultiplier;

    public float preparationDuration;


    public override void Start()
    {
        base.Start();
        canAttack = true;
        coolDownTimer = coolDown - 0.3f;
    }

    public override void FixedUpdate()
    {
        if (stunned == true)
        {
            return;
        }
        if (playerDetected == false)
        {
            Detection();
        }
        else
        {
            target = pTransform.position;
            lookAt = MovementController.mC.transform.position - transform.position;
            
            for (int i = 0; i < ennemiDetection.ennemiControllers.Count; i++)
            {
                target += (Vector2)(transform.position - ennemiDetection.ennemiControllers[i].transform.position).normalized * 3f; //distance ennemis entre eux
            }
            

            if (Vector2.Distance(target, transform.position) >= 0.5f && stunned == false)
            {
                base.FixedUpdate();
            }
            

            if (lookAt.x < 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }


            if (Vector2.Distance(transform.position, pTransform.position) <= range * 2f && canAttack)
            {
                if (coolDownTimer >= coolDown)
                {

                    if (lookAt.x >= 0)
                        currentDash = dashImpulsion;
                    else if (lookAt.x < 0)
                        currentDash = -dashImpulsion;
                    StartCoroutine(PrepareAttack());
                    canAttack = false;
                    coolDownTimer = 0;
                }
                else
                {
                    coolDownTimer += Time.fixedDeltaTime;
                }
            }
            else if (canAttack== false)
                animator.SetBool("Attack", false);

        }
    }


    IEnumerator PrepareAttack ()
    {
        animator.SetBool("IsPreparing", true);
        stunned = true;
        yield return new WaitForSeconds(preparationDuration);
        StartCoroutine(JumpAttack(currentDash));
    }

    IEnumerator JumpAttack(Vector2 dashDirection)
    {
        animator.SetBool("IsPreparing", false);
        animator.SetBool("Attack", true);
        slash.SetActive(true);
        Debug.Log("attack");

            for (float i = duration + momentumMultiplier; i >= momentumMultiplier; i -= Time.fixedDeltaTime)
            {
                rb.velocity = dashDirection.normalized * movementForce * i * Time.fixedDeltaTime;

            }


        //yield return new WaitForSeconds(duration);
        yield return null;
    }

    void EndAttack()
    {
        slash.SetActive(false);
        stunned = false;
        canAttack = true;
        animator.SetBool("Attack", false);
    }

}
