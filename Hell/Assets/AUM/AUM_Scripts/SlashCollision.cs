using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashCollision : MonoBehaviour
{
    public static SlashCollision sc;

    public float pushForce;

    public float duration;

    public float movementForce;

    public float momentumMultiplier;

    private void Start()
    {
        sc = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MovementController.mC.StartCoroutine(MiniDash((InputListener.iL.directionVector).normalized, collision.attachedRigidbody));

        //EnemyController ec = collision.GetComponent<EnemyController>();

        /*if (ec != null)
        {
            ec.TakeDamage(1);
            ec.TakeForce(SwordSlashInstancier.ssi.attackDirection, pushForce);
        }*/
    }

    IEnumerator MiniDash(Vector2 dashDirection, Rigidbody2D ennemiRigidbody2D)
    {
        MovementController.mC.stuned = true;

        for (float i = duration + momentumMultiplier; i >= momentumMultiplier; i -= Time.fixedDeltaTime)
        { 
            ennemiRigidbody2D.velocity = dashDirection * movementForce * 2f * i * Time.fixedDeltaTime;

            MovementController.mC.rb.velocity = dashDirection * movementForce * i * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        MovementController.mC.stuned = false;


        BetterJump.bj.StopLastChangeFall();

        BetterJump.bj.lastChangeFall = BetterJump.bj.StartCoroutine( BetterJump.bj.ChangeFallMultiplier(0.3f, BetterJump.bj.fallMultiplier/5f) );


        MovementController.mC.StopLastChangeSpeed();

        MovementController.mC.lastChangeSpeed =  MovementController.mC.StartCoroutine(MovementController.mC.ChangeSpeed(0.3f, MovementController.mC.speed/5f));
    }
}
