using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSlashCollision : MonoBehaviour
{
    public static BloodSlashCollision bsc;

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
        MovementController.mC.StartCoroutine(AttackMiniDash((InputListener.iL.directionVector).normalized, collision.attachedRigidbody));

        //EnemyController ec = collision.GetComponent<EnemyController>();

        /*if (ec != null)
        {
            ec.TakeDamage(1);
            ec.TakeForce(SwordSlashInstancier.ssi.attackDirection, pushForce);
        }*/
    }

    IEnumerator AttackMiniDash(Vector2 dashDirection, Rigidbody2D ennemiRigidbody2D)
    {
        for (float i = duration + momentumMultiplier; i >= momentumMultiplier; i -= Time.fixedDeltaTime)
        {
            ennemiRigidbody2D.velocity = dashDirection * movementForce * 1.3f * i * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
    }
}
