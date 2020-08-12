using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiSlashCollision : MonoBehaviour
{

    public bool reflected = false;
    public EnnemiThreeBehavior etb;
    public bool isCentaur;

    [Space(10)]
    [Header("Sounds")]
    public AK.Wwise.Event attackParryAudio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collision avec le layer Parry
        if (collision.gameObject.layer == 13)
        {
            attackParryAudio.Post(gameObject);
            FXManager.fxm.fxInstancier(4, gameObject.transform, BaseSlashInstancier.bsi.attackDirectionAngle + Random.Range(-10, 10));

            //Parry.p.StopParry();

            reflected = true;

            FreezTimeManager.ftm.StartCoroutine(FreezTimeManager.ftm.FreezeTimeFor(0.5f, 0.25f));

            if(isCentaur==false)
                etb.PushedBack();

            HealthManager.hm.StartCoroutine(HealthManager.hm.TakeDamage(0));
            Vector2 v = new Vector2(2, 1);
            Vector2 v2 = new Vector2(-2, 1);
            if (collision.transform.position.x - transform.position.x >= 0)
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(v, MovementController.mC.rb, 0.3f, 4700, 0.1f));
            else if (collision.transform.position.x - transform.position.x < 0)
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(v2, MovementController.mC.rb, 0.3f, 4700, 0.1f));

        }

        //Collision avec le layer Player
        if (collision.gameObject.layer == 11 && reflected == false)
        {
            HealthManager.hm.StartCoroutine(HealthManager.hm.TakeDamage(1));
            Vector2 v = new Vector2(1, 0.5f);
            Vector2 v2 = new Vector2(-1, 0.5f);
            if (collision.transform.position.x - transform.position.x >= 0)
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(v, MovementController.mC.rb, 0.3f, 4700, 0.1f));
            else if (collision.transform.position.x - transform.position.x < 0)
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(v2, MovementController.mC.rb, 0.3f, 4700, 0.1f));

        }

        /*
        if (reflected == true)
        {
            if (collision.gameObject.layer == 10)
            {
                EnnemiController ec = collision.GetComponent<EnnemiController>();

                ec.StartCoroutine(ec.DamageDash((ennemiLauncheFrom.transform.position - transform.position).normalized, 0.1f, 500f, 1f));

                ec.StartCoroutine(ec.TakeDamage(1));

                Destroy(gameObject);
            }
        }


        //Collision avec le layer Sol
        if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }

        //Collision avec le layer Wall
        if (collision.gameObject.layer == 14)
        {
            Destroy(gameObject);
        }*/
    }
}
