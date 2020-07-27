using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public EnnemiTwoBehaviorTest ennemiLauncheFrom;

    public float existenceTime;
    float existenceTimer;

    bool reflected = false;
    Vector2 v;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        existenceTimer += Time.deltaTime;

        if (existenceTimer >= existenceTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collision avec le layer Sol
        if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }

        //Collision avec le layer Wall
        if (collision.gameObject.layer == 14)
        {
            Destroy(gameObject);
        }

        //Collision avec le layer Player
        if (collision.gameObject.layer == 11)
        {
            HealthManager.hm.StartCoroutine(HealthManager.hm.TakeDamage(1));
            if (collision.transform.position.x - gameObject.transform.position.x <= 0)
            {
                v = new Vector2(-1f, 1f);
            }
            if (collision.transform.position.x - gameObject.transform.position.x > 0)
            {
                v = new Vector2(1f, 1f);
            }
            MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(v.normalized, MovementController.mC.rb, 0.3f, 3300, 0.1f));

            Destroy(gameObject);
        }

        //Collision avec le layer Parry
        if (collision.gameObject.layer == 13 || collision.gameObject.layer == 9)
        {
            //Parry.p.StopParry();

            reflected = true;

            StartCoroutine(FreezTimeManager.ftm.FreezeTimeFor(0.2f, 0.5f));

            rb.velocity = (ennemiLauncheFrom.transform.position - transform.position).normalized * 40f;
        }

        if (reflected == true)
        {
            if (collision.gameObject.layer == 10)
            {
                EnnemiController ec = collision.GetComponent<EnnemiController>();

                ec.StartCoroutine(ec.DamageDash((ennemiLauncheFrom.transform.position - transform.position).normalized, 0.1f, 500f, 1));

                ec.StartCoroutine(ec.TakeDamage(1));

                Destroy(gameObject);
            }
        }
    }
}
