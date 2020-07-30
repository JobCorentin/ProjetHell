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
    [HideInInspector] public float currentstade;
    public bool lookUp;
    public bool canShoot;
    [HideInInspector] public bool canAim;

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


            if (Vector2.Distance(target, transform.position) >= 0.5f)
            {
                base.FixedUpdate();
            }

            if (pTransform.position.x - transform.position.x < 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
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
                if (canShoot)
                {

                    if (coolDownTimer < coolDown)
                    {
                        coolDownTimer += Time.fixedDeltaTime;
                    }
                    else
                    {
                        animator.SetBool("HasShot", false);
                        currentstade = 0;
                        animator.SetBool("Impair", false);
                        animator.SetBool("Pair", false);
                        StartCoroutine(LaunchBullet());
                        coolDownTimer = 0;
                        canShoot = false;
                    }
                }
            }
        }
        if (animator.GetBool("IsAiming") == true && canAim)
        {

            stade = Mathf.Round(Vector2.Angle(sens, pTransform.position - musket.transform.position) / 30);
            if (stade > 3)
                stade = 3;
            if (lookUp == false)
                stade = -stade;
            /*Debug.Log(stade);
            Debug.Log(currentstade);
            Debug.Log(lookUp);*/


            switch (currentstade)
            {
                case 0:
                    animator.SetBool("Pair", true);
                    animator.SetBool("Impair", false);
                    break;
                case 1:
                    animator.SetBool("Impair", true);
                    animator.SetBool("Pair", false);
                    break;
                case 2:
                    animator.SetBool("Pair", true);
                    animator.SetBool("Impair", false);
                    break;
                case 3:
                    animator.SetBool("Impair", true);
                    animator.SetBool("Pair", false);
                    break;
                case -1:
                    animator.SetBool("Impair", true);
                    animator.SetBool("Pair", false);
                    break;
                case -2:
                    animator.SetBool("Pair", true);
                    animator.SetBool("Impair", false);
                    break;
                case -3:
                    animator.SetBool("Impair", true);
                    animator.SetBool("Pair", false);
                    break;

            }
            if (stade == currentstade)
            {
                animator.SetBool("CanDown", false);
                animator.SetBool("CanUp", false);
            }
            else if (currentstade < stade)
            {
                animator.SetBool("CanUp", true);
                Debug.Log("Up");
                currentstade++;
                StartCoroutine(CooldownAim());
                canAim = false;
            }
            else if (currentstade > stade)
            {
                animator.SetBool("CanDown", true);
                Debug.Log("Down");
                currentstade--;
                StartCoroutine(CooldownAim());
                canAim = false;
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
            while ((finalDirectionAttackAngle < 100 && finalDirectionAttackAngle > 80) || (finalDirectionAttackAngle < -80 && finalDirectionAttackAngle > -100))
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

        currentBullet.Orient(finalDirectionAttack);

        arrow.SetActive(false);
        canShoot = true;
        yield break;
    }

    public void ActivateAimArrow()
    {
        arrow.SetActive(true);
    }
    public IEnumerator CooldownAim()
    {
        yield return new WaitForSeconds(0.1f);
        canAim = true;
    }
}
