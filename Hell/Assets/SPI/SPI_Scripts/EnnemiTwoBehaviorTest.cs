using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiTwoBehaviorTest : EnnemiController
{
    public float coolDown;
    float coolDownTimer = 0;

    public GameObject bulletPrefab;

    public GameObject arrow;

    public float bulletForce;

    public float preparationDuration;

    public EnnemiDetection ennemiDetection;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
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
                if (coolDownTimer < coolDown)
                {
                    coolDownTimer += Time.fixedDeltaTime;
                }
                else
                {
                    StartCoroutine(LaunchBullet());
                    coolDownTimer = 0;
                }



            }
        }
    }
    IEnumerator LaunchBullet()
    {
        Vector2 baseDirectionAttack = pTransform.position - transform.position;

        Vector2 finalDirectionAttack = baseDirectionAttack;
        animator.SetBool("IsPreparing", true);
        animator.SetTrigger("StartPreparing");

        arrow.SetActive(true);

        for (float i = preparationDuration; i > 0; i -= Time.deltaTime)
        {
            finalDirectionAttack = (baseDirectionAttack + ((Vector2)(pTransform.position - transform.position) * 10)).normalized;

            float finalDirectionAttackAngle = Vector2.Angle(transform.right, finalDirectionAttack);

            if (finalDirectionAttack.y < 0)
            {
                finalDirectionAttackAngle = -finalDirectionAttackAngle;
            }

            arrow.transform.rotation = Quaternion.Euler(0, 0, finalDirectionAttackAngle);

            yield return null;
        }
        animator.SetBool("IsPreparing", false);
        animator.SetTrigger("IsAttacking");

        E1Bullet currentBullet = Instantiate(bulletPrefab).GetComponent<E1Bullet>();

        currentBullet.ennemiLauncheFrom = this;
        currentBullet.transform.position = transform.position;

        currentBullet.rb.velocity = finalDirectionAttack * bulletForce;

        arrow.SetActive(false);

        yield break;
    }
}
