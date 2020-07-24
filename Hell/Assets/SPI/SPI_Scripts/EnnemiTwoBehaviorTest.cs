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
    public GameObject musket;

    //public EnnemiDetection ennemiDetection;

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
            target = pTransform.position + ((transform.position - pTransform.position).normalized * range);

            /*for (int i = 0; i < ennemiDetection.ennemiControllers.Count; i++)
            {
                target += (Vector2)(transform.position - ennemiDetection.ennemiControllers[i].transform.position).normalized * 3f;
            }*/

            if (Vector2.Distance(target, transform.position) >= 0.5f)
            {
                base.FixedUpdate();
            }

            if (pTransform.position.x - transform.position.x < 0)
            {
                transform.localScale = new Vector3(1f,1f,1f);
            }
            else
            {

                transform.localScale = new Vector3(-1f, 1f, 1f);
            }

            if (Vector2.Distance(transform.position, pTransform.position) <= range * 2f)
            {
                if (coolDownTimer < coolDown)
                {
                    coolDownTimer += Time.fixedDeltaTime;
                }
                else
                {
                    animator.SetBool("HasShot", false);
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


        animator.SetBool("IsAiming", true);
        Debug.Log("aim");
        //arrow.SetActive(true);

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

        animator.SetBool("IsAiming", false);
        animator.SetBool("HasShot", true);

        E2Bullet currentBullet = Instantiate(bulletPrefab).GetComponent<E2Bullet>();

        currentBullet.ennemiLauncheFrom = this;
        currentBullet.transform.position = musket.transform.position;

        currentBullet.rb.velocity = finalDirectionAttack * bulletForce;

        arrow.SetActive(false);

        yield break;
    }
}
