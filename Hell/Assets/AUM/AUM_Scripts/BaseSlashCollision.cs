using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlashCollision : MonoBehaviour
{
    public static BaseSlashCollision bsc;

    public float pushForce;

    public float duration;

    public float movementForce;

    public float momentumMultiplier;

    private void Start()
    {
        bsc = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ennemi")
        {
            EnnemiController ec = collision.GetComponent<EnnemiController>();
            CameraShake.cs.WeakShake();

            if(BloodManager.bm.bloodNumb < BloodManager.bm.bloodNumbMax)
                BloodManager.bm.bloodNumb += 1;

            MovementController.mC.StartCoroutine(AttackMiniDash((InputListener.iL.directionVector).normalized, ec));

            ec.StartCoroutine(ec.TakeDamage(1));
        }

        if (collision.transform.tag == "Props")
        {
            PropsBehaviour pb = collision.GetComponent<PropsBehaviour>();

            pb.StartCoroutine(pb.TakeDamage(1));
        }

        //EnemyController ec = collision.GetComponent<EnemyController>();

        /*if (ec != null)
        {
            ec.TakeDamage(1);
            ec.TakeForce(SwordSlashInstancier.ssi.attackDirection, pushForce);
        }*/
    }

    IEnumerator AttackMiniDash(Vector2 dashDirection, EnnemiController ennemiController)
    {
        ennemiController.stunned = true;

        MovementController.mC.stuned = true;

        Vector2 currentAttackDirection = dashDirection.normalized;

        float attackDirectionAngle = Vector2.Angle(Vector2.right, currentAttackDirection);

        if (currentAttackDirection.y < 0)
        {
            attackDirectionAngle = -attackDirectionAngle;
        }

        if (attackDirectionAngle < 70 && -70 < attackDirectionAngle)
            currentAttackDirection = Vector2.right;
        else if (70 <= attackDirectionAngle && attackDirectionAngle <= 110)
            currentAttackDirection = Vector2.up;
        else if (attackDirectionAngle > 110 || -110 > attackDirectionAngle)
            currentAttackDirection = Vector2.left;
        else if (-110 <= attackDirectionAngle && attackDirectionAngle <= -70)
            currentAttackDirection = Vector2.down;

        for (float i = duration + momentumMultiplier; i >= momentumMultiplier; i -= Time.fixedDeltaTime)
        { 
            ennemiController.rb.velocity = (currentAttackDirection.normalized * 1.5f + dashDirection.normalized).normalized * movementForce * 1.3f * i * Time.fixedDeltaTime;

            //MovementController.mC.rb.velocity = dashDirection * movementForce * i * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        MovementController.mC.stuned = false;

        //BetterJump.bj.StopLastChangeFall();

        //BetterJump.bj.lastChangeFall = BetterJump.bj.StartCoroutine( BetterJump.bj.ChangeFallMultiplier(0.3f, BetterJump.bj.fallMultiplier/10f) );


        //MovementController.mC.StopLastChangeSpeed();

        //MovementController.mC.lastChangeSpeed =  MovementController.mC.StartCoroutine(MovementController.mC.ChangeSpeed(0.3f, MovementController.mC.speed/5f));

        ennemiController.stunned = false;
    }
}
