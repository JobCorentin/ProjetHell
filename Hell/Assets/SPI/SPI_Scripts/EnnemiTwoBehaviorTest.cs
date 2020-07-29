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
    [HideInInspector] public Vector2 sens;
    [HideInInspector] public float stade;
    public bool lookUp;
    public bool canShoot;

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
                arrow.transform.localScale = new Vector3(1f, 1f, 1f);
                sens = Vector2.left;
            }
            else
            {

                transform.localScale = new Vector3(-1f, 1f, 1f);
                arrow.transform.localScale = new Vector3(-1f, -1f, 1f);
                sens = Vector2.right;
            }

            if (Vector2.Distance(transform.position, pTransform.position) <= range * 2f)
            {
                if ( canShoot)
                {

                    if (coolDownTimer < coolDown)
                    {
                        coolDownTimer += Time.fixedDeltaTime;
                    }
                    else
                    {
                        animator.SetBool("HasShot", false);
                        stade = 0;
                        StartCoroutine(LaunchBullet());
                        coolDownTimer = 0;
                        canShoot = false;
                    }
                }
            }
        }
        if (animator.GetBool("IsAiming") == true)
        {

            animator.SetBool("CanDown", false);
            animator.SetBool("CanUp", false);
            stade = Mathf.Round(Vector2.Angle(sens, pTransform.position - musket.transform.position)/30);
            if (lookUp == true)
                stade = -stade;
            Debug.Log(Vector2.Angle(sens, pTransform.position - musket.transform.position));
            if (Vector2.Angle(sens, pTransform.position-musket.transform.position) > 15* stade)
            {

                if (lookUp)
                    animator.SetBool("CanUp", true);
                else
                    animator.SetBool("CanDown", true);
            }
            if (Vector2.Angle(sens, pTransform.position - musket.transform.position) < 15 * stade)
            {
                if (lookUp)
                    animator.SetBool("CanDown", true);
                else
                    animator.SetBool("CanUp", true);
            }

        }
    }
    IEnumerator LaunchBullet()
    {
        Vector2 baseDirectionAttack = pTransform.position - musket.transform.position;

        Vector2 finalDirectionAttack = baseDirectionAttack;


        animator.SetBool("IsAiming", true);

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
            Debug.Log(finalDirectionAttackAngle);
            /*if ((finalDirectionAttackAngle > 120 && finalDirectionAttackAngle < 60) || (finalDirectionAttackAngle > -620 && finalDirectionAttackAngle < -120))
            {
                 finalDirectionAttackAngle = 120;
                 i += Time.deltaTime;
            }*/
            while ((finalDirectionAttackAngle < 120 && finalDirectionAttackAngle > 60) || (finalDirectionAttackAngle < -60 && finalDirectionAttackAngle > -120))
            {
                finalDirectionAttack = (baseDirectionAttack + ((Vector2)(pTransform.position - musket.transform.position) * 10)).normalized;
                finalDirectionAttackAngle = Vector2.Angle(musket.transform.right, finalDirectionAttack);

                arrow.SetActive(false);
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
        canShoot = true;
        yield break;
    }

    public void ActivateAimArrow()
    {
        arrow.SetActive(true);
    }
}
