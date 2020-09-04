using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiFourBehavior : EnnemiController
{

    public EnnemiDetection ennemiDetection;
    public bool canAttack;
    [HideInInspector] public bool charge;
    [HideInInspector] public bool katana;
    public GameObject slash;

    [HideInInspector] public Transform targetTransform;

    [HideInInspector] public Vector2 lookAt;
    [HideInInspector] public Vector2 lastLookAt;

    public LayerMask chargeLayers;
    public float distanceFromWall;

    public GameObject katanaPrefab;
    public float katanaForce;
    public GameObject katanaLauncher;

    public GameObject arrow;
    public GameObject arrow2;

    public GameObject arrowMask;

    /*
    public Vector2 dashImpulsion;
    [HideInInspector] public Vector2 currentDash;
    public float duration;
    public float movementForce;
    public float momentumMultiplier;
    */
    public float preparationDuration;

    [HideInInspector] public Coroutine pattern;
    [HideInInspector] public float seuilPatternCharge;
    [HideInInspector] public float seuilPatternBoth;
    [HideInInspector] public int lastPatternNum;
    [HideInInspector] public bool hasDoneTwice=false;

    public Material deathMaterial;
    public Cinemachine.CinemachineVirtualCamera cmvcam;

    [Space(10)]
    [Header("Sounds")]
    public AK.Wwise.Event centaurIdleAudio;
    public AK.Wwise.Event centaurChargeAudio;
    public AK.Wwise.Event centaurKatanaVoiceAudio;
    public int centaurIdleAudioTimer;
    bool isInAudioCoroutine = false;
    bool canPlayIdleAudio = true;


    public override void Start()
    {
        cmvcam = GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>();

        lookAt = Vector2.left;
        base.Start();
        canAttack = true;
        coolDownTimer = coolDown - 0.3f;
        //currentDash = dashImpulsion;

        arrow.SetActive(false);
        arrow2.SetActive(false);

        type = 4;
    }

    public override void FixedUpdate()
    {
        if (dead == false)
        {
            if (!isInAudioCoroutine && health > 0)
            {
                StartCoroutine(CentaurIdleAudioCooldown());
            }



            if (stunned == true)
            {
                return;
            }



            if (lookAt.x <= 0)
            {
                transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(-1.5f, 1.5f, 1f);
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
                    CalculatePattern();
                }

                if (charge)
                {
                    base.FixedUpdate();

                    if (Vector2.Distance(target, transform.position) < 1.25f * distanceFromWall)
                    {
                        animator.SetBool("IsCharging", false);

                        centaurChargeAudio.Stop(gameObject);
                        canPlayIdleAudio = true;
                    }

                    if (Vector2.Distance(target, transform.position) < distanceFromWall)
                    {
                        charge = false;
                        slash.SetActive(false);
                        StartCoroutine(BetweenAttack());
                    }

                }

                if (katana)
                {

                    lookAt = pTransform.position - transform.position;
                }

            }
        }
    }

    
    IEnumerator PrepareCharge()
    {
        animator.SetBool("IsPreparingCharge", true);
        canPlayIdleAudio = false;

        yield return new WaitForSeconds(preparationDuration);

        slash.SetActive(true);
        //slash.GetComponent<ZombiSlashCollision>().reflected = false;
        charge = true;
        animator.SetBool("IsPreparingCharge", false);
        animator.SetBool("IsCharging", true);

        if (SoundManager.instance.havePlayedBossTheme == false && SoundManager.instance.isBossFight)
        {
            SoundManager.instance.bossTheme.Post(SoundManager.instance.gameObject);
            SoundManager.instance.havePlayedBossTheme = true;
        }


        centaurChargeAudio.Post(gameObject);
    }

    IEnumerator PrepareKatana(bool both)
    {
        animator.SetBool("IsPreparingKatana", true);
        canPlayIdleAudio = false;
        if(lastPatternNum ==0)
            lastLookAt = lookAt;
        katana = true;

        arrow.SetActive(true);
        arrow2.SetActive(true);

        Vector2 attackDirection = (pTransform.position - katanaLauncher.transform.position).normalized;

        for (float i = preparationDuration; i > 0; i -= Time.deltaTime)
        {
            attackDirection = (pTransform.position - katanaLauncher.transform.position).normalized;

            float attackDirectionAngle = Vector2.Angle(transform.right, attackDirection);

            if(attackDirection.x > 0)
            {
                attackDirection = -attackDirection;

                attackDirectionAngle = Vector2.Angle(transform.right, attackDirection);
            }

            if (attackDirection.y < 0)
            {
                attackDirectionAngle = -attackDirectionAngle;
            }

            
            arrow.transform.rotation = Quaternion.Euler(0, 0, attackDirectionAngle);
            //arrow2.transform.rotation = Quaternion.Euler(0, 0, attackDirectionAngle);

            arrowMask.transform.localScale = new Vector2(arrowMask.transform.localScale.x, Mathf.Max(0, 1.6f * (i - Time.deltaTime / preparationDuration)));

            yield return null;
        }

        arrow.SetActive(false);
        arrow2.SetActive(false);

        animator.SetBool("IsPreparingKatana", false);
        if(both)
        {
            animator.SetBool("LaunchBoth", true);
            centaurKatanaVoiceAudio.Post(gameObject);

            if (SoundManager.instance.havePlayedBossTheme == false && SoundManager.instance.isBossFight)
            {
                SoundManager.instance.bossTheme.Post(SoundManager.instance.gameObject);
                SoundManager.instance.havePlayedBossTheme = true;
            }

            //StartCoroutine(LaunchingKatana());
        }
        else
        {
            animator.SetBool("LaunchKatana", true);
            centaurKatanaVoiceAudio.Post(gameObject);

            //StartCoroutine(LaunchingKatana());
        }
    }

    IEnumerator BetweenAttack()
    {
        if (lastPatternNum == 0)
            lookAt = -lookAt;
        else
        {
            lookAt = lastLookAt;
            katana = false;
        }
        centaurChargeAudio.Stop(gameObject);
        canPlayIdleAudio = true;


        yield return new WaitForSeconds(coolDown);
        canAttack = true;
        animator.SetBool("LaunchBoth", false);
        animator.SetBool("LaunchKatana", false);
    }

    IEnumerator LaunchingKatana()
    {
        E2Bullet currentBullet = Instantiate(katanaPrefab).GetComponent<E2Bullet>();
        currentBullet.transform.position = katanaLauncher.transform.position;
        Vector2 attackDirection = pTransform.position - katanaLauncher.transform.position ;
        attackDirection = (attackDirection + ((Vector2)(pTransform.position - katanaLauncher.transform.position) * 10)).normalized;
        currentBullet.Orient(attackDirection);
        currentBullet.rb.velocity = attackDirection * katanaForce;
       yield return null;
    }

    public void StopAttack()
    {
        StopCoroutine(LaunchingKatana());
        StopCoroutine(BetweenAttack());
        StopCoroutine(PrepareCharge());
        StopCoroutine(PrepareKatana(true));
        StopCoroutine(PrepareKatana(false));
        stunned = true;
        charge = false;
        slash.SetActive(false);
        animator.SetBool("IsCharging", false);
        animator.SetBool("LaunchBoth", false);
        animator.SetBool("LaunchKatana", false);
        animator.SetBool("IsPreparingCharge", false);

        centaurChargeAudio.Stop(gameObject);

        canPlayIdleAudio = true;
    }

    public void CalculatePattern()
    {

        seuilPatternCharge = 0.5f;
        seuilPatternBoth = 0.75f;
        if (hasDoneTwice== false)
        {

            if (lastPatternNum == 0) //charge
            {
                seuilPatternCharge += 0.25f;
            }
            else if (lastPatternNum == 1) // one katana
            {
                seuilPatternBoth -= 0.1f;
            }
            else if (lastPatternNum == 2) // both katana
            {
                seuilPatternCharge -= 0.1f;
                seuilPatternBoth += 0.1f;
            }
        } else
        {

            if (lastPatternNum == 0) //charge
            {
                seuilPatternCharge -= 0.2f;
            }
            else if (lastPatternNum == 1) // one katana
            {
                seuilPatternCharge += 0.25f;
                seuilPatternBoth -= 0.05f;
            }
            else if (lastPatternNum == 2) // both katana
            {

                seuilPatternCharge += 0.2f;
                seuilPatternBoth += 0.15f;
            }
        }
        hasDoneTwice = false;



        if (pTransform.position.y > transform.position.y + 5 || pTransform.position.y < transform.position.y - 5) //Si le joueur est suffisamment haut, le centaure lance un katana
        {
            seuilPatternCharge -= 0.15f;

        }
        else
        {

            seuilPatternCharge += 0.15f;
            seuilPatternBoth += 0.5f; 
        }
        if ((pTransform.position.x - transform.position.x) * lookAt.x > 0) // Charge
        {

            seuilPatternCharge += 0.05f;
        }

        if (Vector2.Distance(transform.position, pTransform.position) > range)
        {
            seuilPatternCharge -= 0.1f;
            seuilPatternBoth -= 0.15f;
        }
        if (health <= (health / 3))
        {
            seuilPatternCharge += 0.2f;
        }


        float calc = Random.Range(0.0f, 1f);

        if(calc <= seuilPatternCharge)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, lookAt, Mathf.Infinity, chargeLayers);
            if (hit == true & Vector2.Distance(hit.transform.position, transform.position) > distanceFromWall)
            {
                target = hit.transform.position;
                canAttack = false;
                StartCoroutine(PrepareCharge());

                if (lastPatternNum == 0)
                    hasDoneTwice = true;
                lastPatternNum = 0;

            } else
            {
                lookAt = -lookAt;
                BetweenAttack();
            }

        } else
        {
            if (calc >= seuilPatternBoth)
            {
                canAttack = false;
                StartCoroutine(PrepareKatana(true));
                if (lastPatternNum == 2)
                    hasDoneTwice = true;
                lastPatternNum = 2;
            } else
            {
                canAttack = false;
                StartCoroutine(PrepareKatana(false));
                if (lastPatternNum == 1)
                    hasDoneTwice = true;
                lastPatternNum = 1;
            }
        }

    }

    /*
    void EndAttack()
    {
        //slash.SetActive(false);
        stunned = false;
        canAttack = true;
        animator.SetBool("Attack", false);
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




    IEnumerator CentaurIdleAudioCooldown()
    {
        isInAudioCoroutine = true;
        yield return new WaitForSeconds(Random.Range(centaurIdleAudioTimer - (centaurIdleAudioTimer / 2), centaurIdleAudioTimer + (centaurIdleAudioTimer / 2)));
        if (health > 0 && canPlayIdleAudio)
        {
            centaurIdleAudio.Post(gameObject);
        }
        isInAudioCoroutine = false;
    }

    public IEnumerator ChangeCamera()
    {
        dead = true;
        stunned = true;
        SoundManager.instance.levelTheme.Stop(SoundManager.instance.gameObject);

        MovementController.mC.stuned = true;

        cmvcam.Follow = transform;

        float sizeMultiplicator = 0.5f;

        float endSize = cmvcam.m_Lens.OrthographicSize * sizeMultiplicator;

        float timer = 0f;

        while (!Mathf.Approximately(cmvcam.m_Lens.OrthographicSize, endSize) && timer <= 1.5f)
        {
            MovementController.mC.stuned = true;

            cmvcam.m_Lens.OrthographicSize = Mathf.SmoothStep(cmvcam.m_Lens.OrthographicSize, endSize, 3f * Time.deltaTime);

            timer += Time.deltaTime;

            yield return null;
        }
    }

    public void ActivateDeathBackground()
    {
        HealthManager.hm.deathBackgroundAnimator.SetTrigger("Die");
        SoundManager.instance.bossTheme.Stop(SoundManager.instance.gameObject);

        sr.sortingOrder = 401;

        sr.material = deathMaterial;
    }
}
