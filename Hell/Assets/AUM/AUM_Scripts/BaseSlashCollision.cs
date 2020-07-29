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

    public bool ennemiTouched = false;

    [HideInInspector] public bool bouncing;

    [HideInInspector] public Coroutine lastBounce;

    private void Awake()
    {
        bsc = this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ennemi")
        {
            FXManager.fxm.fxInstancier(2, collision.transform, BaseSlashInstancier.bsi.attackDirectionAngle + Random.Range(-10,10));

            ennemiTouched = true;
            EnnemiController ec = collision.GetComponent<EnnemiController>();
            CameraShake.cs.WeakShake();

            if(BloodManager.bm.bloodNumb < BloodManager.bm.bloodNumbMax)
                BloodManager.bm.bloodNumb += 1;

            MovementController.mC.StartCoroutine(AttackMiniDash((InputListener.iL.directionVector).normalized, ec));

            ec.StartCoroutine(ec.TakeDamage(1));
            FreezTimeManager.ftm.StartCoroutine(FreezTimeManager.ftm.FreezeTimeFor(0.045f,0f));

            if(BaseSlashInstancier.bsi.canBounce)
            {
                if(lastBounce != null)
                {
                    StopCoroutine(lastBounce);
                }

                lastBounce = MovementController.mC.StartCoroutine(Bounce());
            }
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

    public IEnumerator Bounce()
    {
        BetterJump.bj.lowJumpMultiplier = 1;

        bouncing = true;

        MovementController.mC.rb.velocity = Vector2.zero;

        MovementController.mC.rb.AddForce(Vector2.up * (MovementController.mC.jumpForce) * Time.fixedDeltaTime, ForceMode2D.Impulse);

        //yield return new WaitForSeconds(BaseSlashInstancier.bsi.duration * 2f);

        for (float i = BaseSlashInstancier.bsi.duration * 2f; i > 0; i -= Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
        }

        BetterJump.bj.lowJumpMultiplier = BetterJump.bj.baseLowJumpMultiplier;

        bouncing = false;
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
