using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2Bullet : MonoBehaviour
{

    public Rigidbody2D rb;
    

    public float existenceTime;
    float existenceTimer;


    [Space(10)]
    [Header("Sounds")]
    public AK.Wwise.Event katanaIdleAudio;
    public AK.Wwise.Event katanaImpactAudio;


    private void Start()
    {
        katanaIdleAudio.Post(gameObject);
    }

    void Update()
    {
        existenceTimer += Time.deltaTime;

        if (existenceTimer >= existenceTime)
        {
            Destroy(gameObject);
            katanaIdleAudio.Stop(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collision avec le layer Sol
        if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
            katanaIdleAudio.Stop(gameObject);
            katanaImpactAudio.Post(gameObject);
        }

        //Collision avec le layer Wall
        if (collision.gameObject.layer == 14)
        {
            Destroy(gameObject);
            katanaIdleAudio.Stop(gameObject);
            katanaImpactAudio.Post(gameObject);
        }

        if (collision.gameObject.layer == 11)
        {


            HealthManager.hm.StartCoroutine(HealthManager.hm.TakeDamage(1));
            Vector2 v = new Vector2(1, 1);
            Vector2 v2 = new Vector2(-1, 1);
            if (collision.transform.position.x - transform.position.x >= 0)
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(v, MovementController.mC.rb, 0.3f, 3300, 0.1f));
            else if (collision.transform.position.x - transform.position.x < 0)
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(v2, MovementController.mC.rb, 0.3f, 3300, 0.1f));


            Destroy(gameObject);
            katanaIdleAudio.Stop(gameObject);


        }

        //Collision avec le layer Parry
        if (collision.gameObject.layer == 13)
        {
            katanaIdleAudio.Stop(gameObject);

            FXManager.fxm.fxInstancier(4, gameObject.transform, BaseSlashInstancier.bsi.attackDirectionAngle + Random.Range(-10, 10));

            //Parry.p.StopParry();


            FreezTimeManager.ftm.StartCoroutine(FreezTimeManager.ftm.FreezeTimeFor(0.2f, 5f));

            Vector2 v = new Vector2(1, 0.5f);
            Vector2 v2 = new Vector2(-1, 0.5f);

            MovementController.mC.StartCoroutine(MovementController.mC.Pushed(0.3f));
            if (collision.transform.position.x - transform.position.x >= 0)
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(v, MovementController.mC.rb, 0.3f, 4700, 0.1f));
            else if (collision.transform.position.x - transform.position.x < 0)
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(v2, MovementController.mC.rb, 0.3f, 4700, 0.1f));

            Destroy(gameObject);
        }
    }

    public void Orient(Vector2 currentAttackDirection)
    {
        if (currentAttackDirection.x > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

}
