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
        if(collision.transform.tag == "Ennemi")
        {
            EnnemiController ec = collision.GetComponent<EnnemiController>();
            CameraShake.cs.StrongShake();

            MovementController.mC.StartCoroutine(AttackMiniDash((InputListener.iL.directionVector).normalized, ec ));

            ec.StartCoroutine(ec.TakeDamage(2));

            
        }

        if(collision.transform.tag == "Props")
        {
            PropsBehaviour pb = collision.GetComponent<PropsBehaviour>();

            pb.StartCoroutine(pb.TakeDamage(2));
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

        for (float i = duration + momentumMultiplier; i >= momentumMultiplier; i -= Time.fixedDeltaTime)
        {
            ennemiController.rb.velocity = dashDirection * movementForce * 1.3f * i * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        ennemiController.stunned = false;
    }
}
