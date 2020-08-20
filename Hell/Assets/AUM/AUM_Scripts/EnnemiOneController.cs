using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiOneController : EnnemiController
{

    public GameObject bulletPrefab;

    public GameObject arrow;
    public GameObject arrow2;

    public float bulletForce;

    public float preparationDuration;

    public EnnemiDetection ennemiDetection;

    Coroutine lastLaunchBullet;

    public bool isTypeB;
    [HideInInspector] public bool isAttacking;



    [Space(10)]
    [Header("Sounds")]
    public AK.Wwise.Event hearthLaunchAttackAudio;
    public AK.Wwise.Event hearthAttackAudio;
    public AK.Wwise.Event hearthIdleAudio;
    public AK.Wwise.Event hearthDamagesAudio;
    public AK.Wwise.Event hearthDeathAudio;
    public int hearthIdleAudioTimer;
    bool isInAudioCoroutine = false;
    bool canPlayDeathAudio = true;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        type = 0;

        arrow.SetActive(false);
        arrow2.SetActive(false);
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (!isInAudioCoroutine && health > 0)
        {
            StartCoroutine(HearthIdleAudioCooldown());
        }
        if (health <= 0 && canPlayDeathAudio)
        {
            hearthDeathAudio.Post(gameObject);
            canPlayDeathAudio = false;
        }



        CheckingIfAlive();

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
                target = pTransform.position + ((transform.position - pTransform.position).normalized * range);

                for (int i = 0; i < ennemiDetection.ennemiControllers.Count; i++)
                {
                    target += (Vector2)(transform.position - ennemiDetection.ennemiControllers[i].transform.position).normalized * 3f;
                }

                if (Vector2.Distance(target, transform.position) >= 0.5f)
                {
                    base.FixedUpdate();
                }

                if (Vector2.Distance(transform.position, pTransform.position) <= range * 2f)
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
                    if (isAttacking == false)
                    {
                        if (coolDownTimer >= coolDown && numbWhoHasAttacked < numberBetweenGroupAttack && canAttack)
                        {
                            lastLaunchBullet = StartCoroutine(LaunchBullet());
                            coolDownTimer = 0;
                        }
                        else
                        {
                            coolDownTimer += Time.fixedDeltaTime;
                        }
                    }


                }

                //if(Vector2.Distance(transform.position, pTransform.position))
                //rb.AddForce(pTransform)
            }
        }



    }

    public override IEnumerator TakeDamage(int amount)
    {
        return base.TakeDamage(amount);
    }

    IEnumerator LaunchBullet()
    {
        isAttacking = true;
        StartCoroutine(HasAttackedFor(timeBetweenGroupAttack));

        Vector2 baseDirectionAttack = pTransform.position - transform.position;

        Vector2 finalDirectionAttack = baseDirectionAttack;
        animator.SetBool("IsPreparing",true);
        animator.SetTrigger("StartPreparing");
        hearthLaunchAttackAudio.Post(gameObject);

        arrow.SetActive(true);
        arrow2.SetActive(true);

        for (float i = 0; i < preparationDuration; i += Time.deltaTime)
        {
            finalDirectionAttack = ( baseDirectionAttack + ((Vector2)(pTransform.position - transform.position) * 10) ).normalized;

            float finalDirectionAttackAngle = Vector2.Angle(transform.right, finalDirectionAttack);

            if(finalDirectionAttack.y < 0)
            {
                finalDirectionAttackAngle = -finalDirectionAttackAngle;
            }

            arrow.transform.rotation = Quaternion.Euler(0, 0, finalDirectionAttackAngle);
            arrow2.transform.rotation = Quaternion.Euler(0, 0, finalDirectionAttackAngle);

            arrow2.transform.localScale = new Vector2(i / preparationDuration, arrow2.transform.localScale.y);

            yield return null;
        }
        animator.SetBool("IsPreparing", false);
        animator.SetTrigger("IsAttacking");
        hearthAttackAudio.Post(gameObject);
        hearthLaunchAttackAudio.Stop(gameObject);

        
        E1Bullet currentBullet = Instantiate(bulletPrefab).GetComponent<E1Bullet>();

        currentBullet.ennemiLauncheFrom = this;
        currentBullet.transform.position = transform.position;

        if (isTypeB)
        {
            E1Bullet currentBulletB = Instantiate(bulletPrefab).GetComponent<E1Bullet>();
            currentBulletB.ennemiLauncheFrom = this;
            currentBulletB.transform.position = transform.position;

            E1Bullet currentBulletC = Instantiate(bulletPrefab).GetComponent<E1Bullet>();
            currentBulletC.ennemiLauncheFrom = this;
            currentBulletC.transform.position = transform.position;

            float finalDirectionAttackAngleB = Vector2.Angle(transform.right, finalDirectionAttack);
            float finalDirectionAttackAngleC = Vector2.Angle(transform.right, finalDirectionAttack);



            Vector2 finalDirectionAttackB = (finalDirectionAttack + ((Vector2.one) * 0.2f)).normalized;
            Vector2 finalDirectionAttackC = (finalDirectionAttack + ((Vector2.one) * -0.2f)).normalized;

            currentBulletB.rb.velocity = finalDirectionAttackB * bulletForce;
            currentBulletC.rb.velocity = finalDirectionAttackC * bulletForce;

        }
        currentBullet.rb.velocity = finalDirectionAttack * bulletForce;

        arrow.SetActive(false);
        arrow2.SetActive(false);
        isAttacking = false;
        yield break;
    }

    public void StopLaunchBullet()
    {
        animator.SetBool("IsPreparing", false);
        hearthLaunchAttackAudio.Stop(gameObject);
        arrow.SetActive(false);
        arrow2.SetActive(false);
        if(lastLaunchBullet != null)
            StopCoroutine(lastLaunchBullet);

        isAttacking = true;
    }

    public void dieHeart()
    {
        BoxCollider2D bc2 = gameObject.GetComponent<BoxCollider2D>();
        bc2.enabled = false;

        hearthDeathAudio.Post(gameObject);
    }


    IEnumerator HearthIdleAudioCooldown()
    {
        isInAudioCoroutine = true;
        yield return new WaitForSeconds(Random.Range(hearthIdleAudioTimer - (hearthIdleAudioTimer / 2), hearthIdleAudioTimer + (hearthIdleAudioTimer / 2)));
        if (health > 0)
        {
            hearthIdleAudio.Post(gameObject);
        }
        isInAudioCoroutine = false;
    }
}
