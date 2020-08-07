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
    [HideInInspector] public Vector2 lastLookAt;

    public LayerMask chargeLayers;
    public float distanceFromWall;

    public GameObject katanaPrefab;
    public float katanaForce;
    public GameObject katanaLauncher;
    [HideInInspector] public bool lastPatternWasCharge;

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
        //currentDash = dashImpulsion;
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

            /*for (int i = 0; i < ennemiDetection.ennemiControllers.Count; i++)
            {
                target += (Vector2)(transform.position - ennemiDetection.ennemiControllers[i].transform.position).normalized * 3f; //distance ennemis entre eux
            }*/

            if (canAttack)
            {
                if (pTransform.position.y > transform.position.y + 5 || pTransform.position.y < transform.position.y - 5) //Si le joueur est suffisamment haut, le centaure lance un katana
                {
                    target = pTransform.position;
                    canAttack = false;
                    StartCoroutine(PrepareKatana());
                }
                else if ((pTransform.position.x - transform.position.x) * lookAt.x > 0) // Charge
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, lookAt, Mathf.Infinity, chargeLayers);
                    if (hit == true & Vector2.Distance(hit.transform.position, transform.position) > distanceFromWall)
                    {
                        target = hit.transform.position;
                        canAttack = false;
                        StartCoroutine(PrepareCharge());
                    }
                }
                else
                {
                    target = pTransform.position;
                    canAttack = false;
                    StartCoroutine(PrepareKatana());
                }
            }

            if (charge)
            {
                base.FixedUpdate();

                if (Vector2.Distance(target, transform.position) < 1.25f*distanceFromWall)
                    animator.SetBool("IsCharging", false);
                if (Vector2.Distance(target, transform.position) < distanceFromWall)
                {
                    charge = false;
                    slash.SetActive(false);
                    StartCoroutine(BetweenAttack());
                }

            }
       
        }
    }

    
    IEnumerator PrepareCharge()
    {
        animator.SetBool("IsPreparingCharge", true);
        yield return new WaitForSeconds(preparationDuration);
        slash.SetActive(true);
        charge = true;
        animator.SetBool("IsPreparingCharge", false);
        animator.SetBool("IsCharging", true);
        lastPatternWasCharge = true;
    }
    IEnumerator PrepareKatana()
    {
        animator.SetBool("IsPreparingKatana", true);
        lastLookAt = lookAt;
        lookAt = pTransform.position - transform.position;
        yield return new WaitForSeconds(preparationDuration);
        animator.SetBool("IsPreparingKatana", false);
        lastPatternWasCharge = false;
        if(Vector2.Distance(transform.position, pTransform.position) > range)
        {
            animator.SetBool("LaunchBoth", true);
            //StartCoroutine(LaunchingKatana());
        }
        else
        {
            animator.SetBool("LaunchKatana", true);
            //StartCoroutine(LaunchingKatana());
        }
    }

    IEnumerator BetweenAttack()
    {
        if (lastPatternWasCharge)
            lookAt = -lookAt;
        else
            lookAt = lastLookAt;
        yield return new WaitForSeconds(coolDown);
        canAttack = true;
        animator.SetBool("LaunchBoth", false);
        animator.SetBool("LaunchKatana", false);
    }

    IEnumerator LaunchingKatana()
    {
        E2Bullet currentBullet = Instantiate(katanaPrefab).GetComponent<E2Bullet>();
        currentBullet.transform.position = katanaLauncher.transform.position;
        Vector2 attackDirection = pTransform.position - katanaLauncher.transform.position;
        currentBullet.Orient(attackDirection);
        currentBullet.rb.velocity = attackDirection * katanaForce;
        yield return null;
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
