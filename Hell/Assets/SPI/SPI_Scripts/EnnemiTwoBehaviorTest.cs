using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiTwoBehaviorTest : EnnemiController
{

    public GameObject bulletPrefab;

    public GameObject arrow;
    public GameObject arrow2;

    public float bulletForce;

    public float preparationDuration;
    public GameObject musket;
    [HideInInspector] public Vector2 sens;
    [HideInInspector] public float stade;
    [HideInInspector] public float currentstade;
    public bool lookUp;
    public bool canShoot;
    [HideInInspector] public bool canAim;

    Coroutine lastLaunchBullet;

    //public EnnemiDetection ennemiDetection;





    [Space(10)]
    [Header("Sounds")]
    public AK.Wwise.Event musketAimAudio;
    public AK.Wwise.Event musketShotAudio;
    public AK.Wwise.Event musketIdleAudio;
    public AK.Wwise.Event musketDeathAudio;
    public int musketIdleAudioTimer;
    bool isInAudioCoroutine = false;
    bool canPlayDeathAudio = true;


    public override void Start()
    {
        base.Start();
        coolDownTimer = coolDown-0.3f;
        type = 1;

    }

    public override void FixedUpdate()
    {
        CheckingIfAlive();

        if (!isInAudioCoroutine && health > 0)
        {
            StartCoroutine(MusketIdleAudioCooldown());
        }
        if (health <= 0 && canPlayDeathAudio)
        {
            musketDeathAudio.Post(gameObject);
            canPlayDeathAudio = false;
        }


        if (hasSpawn == true)
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
                target = MovementController.mC.transform.position + ((transform.position - MovementController.mC.transform.position).normalized * range);


                if (Vector2.Distance(target, transform.position) >= 0.5f)
                {
                    base.FixedUpdate();
                }

                if (MovementController.mC.transform.position.x - transform.position.x < 0)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    arrow.transform.localScale = new Vector3(1f, 1f, 1f);
                    arrow2.transform.localScale = new Vector3(1f, 1f, 1f);
                    sens = Vector2.left;
                }
                else
                {

                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    arrow.transform.localScale = new Vector3(-1f, -1f, 1f);
                    arrow2.transform.localScale = new Vector3(-1f, -1f, 1f);
                    sens = Vector2.right;
                }

                if (Vector2.Distance(transform.position, MovementController.mC.transform.position) <= range * 2f)
                {
                    if (canShoot)
                    {
                        numbWhoHasAttacked = 0;
                        bool canAttack = true;

                        for (int i = 0; i < ennemy_Controllers.Length; i++)
                        {
                            if (ennemy_Controllers[i].hasAttacked == true && ennemy_Controllers[i].gameObject.activeSelf == true)
                            {
                                numbWhoHasAttacked++;
                            }

                            if (ennemy_Controllers[i].coolDownTimer > ennemy_Controllers[i].coolDown && ennemy_Controllers[i].gameObject.activeSelf == true)
                            {
                                if (Vector2.Distance(MovementController.mC.rb.transform.position, ennemy_Controllers[i].transform.position) <
                                Vector2.Distance(MovementController.mC.rb.transform.position, transform.position))
                                {
                                    canAttack = false;
                                }
                            }

                        }

                        if (coolDownTimer >= coolDown && numbWhoHasAttacked < numberBetweenGroupAttack && canAttack)
                        {
                            animator.SetBool("HasShot", false);
                            currentstade = 0;
                            lastLaunchBullet = StartCoroutine(LaunchBullet());
                            canAim = true;
                            coolDownTimer = 0;
                            canShoot = false;
                        }
                        else
                        {
                            coolDownTimer += Time.fixedDeltaTime;
                        }


                    }
                }
            }
            if (animator.GetBool("IsAiming") == true && canAim)
            {

                stade = Mathf.Round(Vector2.Angle(sens, pTransform.position - musket.transform.position) / 15);
                if (stade > 3)
                    stade = 3;
                if (lookUp == false)
                    stade = -stade;

                if (stade == currentstade)
                {
                    animator.SetBool("CanDown", false);
                    animator.SetBool("CanUp", false);
                }
                else if (currentstade < stade)
                {
                    animator.SetBool("CanUp", true);
                    animator.SetBool("CanDown", false);
                    currentstade++;
                    /*StartCoroutine(CooldownAim());*/
                    canAim = false;
                }
                else if (currentstade > stade)
                {
                    animator.SetBool("CanDown", true);
                    animator.SetBool("CanUp", false);
                    currentstade--;
                    /*StartCoroutine(CooldownAim());*/
                    canAim = false;
                }
            }
        }
    }
    IEnumerator LaunchBullet()
    {
        StartCoroutine(HasAttackedFor(timeBetweenGroupAttack));

        Vector2 baseDirectionAttack = pTransform.position - musket.transform.position;

        Vector2 finalDirectionAttack = baseDirectionAttack;

        animator.SetBool("IsAiming", true);
        musketAimAudio.Post(gameObject);

        for (float i = preparationDuration; i > 0; i -= Time.deltaTime)
        {
            finalDirectionAttack = (baseDirectionAttack + ((Vector2)(pTransform.position - musket.transform.position) * 10)).normalized;

            float finalDirectionAttackAngle = Vector2.Angle(musket.transform.right, finalDirectionAttack);

            if (finalDirectionAttack.y < 0)
            {
                finalDirectionAttackAngle = -finalDirectionAttackAngle;
                lookUp = false;

            }
            if (finalDirectionAttack.y > 0)
            {
                lookUp = true;

            }
            while ((finalDirectionAttackAngle < 100 && finalDirectionAttackAngle > 80) || (finalDirectionAttackAngle < -80 && finalDirectionAttackAngle > -100))
            {
                finalDirectionAttack = (baseDirectionAttack + ((Vector2)(pTransform.position - musket.transform.position) * 10)).normalized;
                finalDirectionAttackAngle = Vector2.Angle(musket.transform.right, finalDirectionAttack);

                arrow.SetActive(false);
                arrow2.SetActive(false);
                if (finalDirectionAttack.y < 0)
                {
                    finalDirectionAttackAngle = -finalDirectionAttackAngle;
                    lookUp = false;

                }
                if (finalDirectionAttack.y > 0)
                {
                    lookUp = true;

                }
                yield return null;
            }
            arrow.SetActive(true);
            arrow2.SetActive(true);
            arrow.transform.rotation = Quaternion.Euler(0, 0, finalDirectionAttackAngle - (4 * i));
            arrow2.transform.rotation = Quaternion.Euler(0, 0, finalDirectionAttackAngle + (4 * i));
            yield return null;
        }

        animator.SetBool("IsAiming", false);
        animator.SetBool("HasShot", true);

        E1Bullet currentBullet = Instantiate(bulletPrefab).GetComponent<E1Bullet>();

        currentBullet.ennemiLauncheFrom = this;
        currentBullet.transform.position = musket.transform.position;
        musketShotAudio.Post(gameObject);

        currentBullet.rb.velocity = finalDirectionAttack * bulletForce;

        currentBullet.Orient(finalDirectionAttack);

        arrow.SetActive(false);
        arrow2.SetActive(false);
        canShoot = true;
        animator.SetBool("Impair", false);
        animator.SetBool("Pair", false);
        yield break;
    }

    public void ActivateAimArrow()
    {
        arrow.SetActive(true);
    }
    public void Impair()
    {
        animator.SetBool("Impair", true);
        animator.SetBool("Pair", false);
        animator.SetBool("CanUp", false);
        animator.SetBool("CanDown", false);
        canAim = true;
    }
    public void Pair()
    {
        animator.SetBool("Pair", true);
        animator.SetBool("Impair", false);
        animator.SetBool("CanUp", false);
        animator.SetBool("CanDown", false);
        canAim = true;
    }
    
    public IEnumerator CooldownAim()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("CanUp", false);
        animator.SetBool("CanDown", false);
        canAim = true;
    }

    public void StopLaunchBullet()
    {
        animator.SetBool("IsAiming", false);
        musketAimAudio.Stop(gameObject);

        arrow.SetActive(false);
        arrow2.SetActive(false);

        if (lastLaunchBullet != null)
            StopCoroutine(lastLaunchBullet);
    }
    public void StopMusket()
    {
        animator.SetBool("IsAiming", false);

        arrow.SetActive(false);
        arrow2.SetActive(false);

        if (lastLaunchBullet != null)
            StopCoroutine(lastLaunchBullet);
        StopCoroutine(CooldownAim());
    }
    IEnumerator MusketIdleAudioCooldown()
    {
        isInAudioCoroutine = true;
        yield return new WaitForSeconds(Random.Range(musketIdleAudioTimer - (musketIdleAudioTimer / 2), musketIdleAudioTimer + (musketIdleAudioTimer / 2)));
        if (health > 0)
        {
            musketIdleAudio.Post(gameObject);
        }
        isInAudioCoroutine = false;
    }
}
