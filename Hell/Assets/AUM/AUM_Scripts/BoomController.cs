using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomController : MonoBehaviour
{
    [HideInInspector] public Vector2 target;

    [HideInInspector] public float speed;

    public Rigidbody2D rb;

    public SpriteRenderer sr;

    public Collider2D col;
    public Collider2D physicCol;

    [HideInInspector] public Coroutine lastCoroutine;

    [HideInInspector] public bool touchedEnnemy = false;


    [Space(10)]
    [Header("Sounds")]
    public AK.Wwise.Event swordIdleAudio;



    // Start is called before the first frame update
    void Start()
    {
        swordIdleAudio.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        /*if(touchedEnnemy == true)
        {
            sr.color = Color.blue;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ennemi")
        {
            EnnemiController ec = collision.GetComponent<EnnemiController>();

            if (ec.dead == true || ec.hasSpawn == false)
                return;

            FXManager.fxm.fxInstancier(2, collision.transform, BaseSlashInstancier.bsi.attackDirectionAngle + Random.Range(-10, 10));
            FXManager.fxm.fxInstancier(4, collision.transform, 0);
            
            CameraShake.cs.WeakShake();

            MovementController.mC.StartCoroutine(BaseSlashCollision.bsc.AttackMiniDash((rb.velocity).normalized, ec, 500f));

            ec.StartCoroutine(ec.TakeDamage(1));
            FreezTimeManager.ftm.StartCoroutine(FreezTimeManager.ftm.FreezeTimeFor(0.045f, 0f));

            touchedEnnemy = true;

            if (ec.type == 0)
            {
                EnnemiOneController eoc = ec.GetComponent<EnnemiOneController>();

                eoc.StopLaunchBullet();
            }

            if (ec.type == 1)
            {
                EnnemiTwoBehaviorTest etc = ec.GetComponent<EnnemiTwoBehaviorTest>();

                etc.StopMusket();
            }
            if (ec.type == 2)
            {
                EnnemiThreeBehavior erc = ec.GetComponent<EnnemiThreeBehavior>();

                erc.StopAttack();
            }


        }

        if (collision.transform.tag == "Props")
        {
            PropsBehaviour pb = collision.GetComponent<PropsBehaviour>();

            pb.StartCoroutine(pb.TakeDamage(1));
        }
    }

    public IEnumerator GoToTargetThenPlayer()
    {
        GainLife.gl.noSword = true;

        float temp = 0;

        while(Vector2.Distance(transform.position, target) > 10f && temp < 0.5f)
        {
            rb.velocity = (target -(Vector2)transform.position).normalized * speed;

            temp += Time.deltaTime;

            yield return null;
        }

        

        for (float i = 0.2f; i > 0; i -= Time.deltaTime)
        {
            rb.velocity = (target - (Vector2)transform.position).normalized * speed * (i * 5f);

            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        col.enabled = false;

        yield return null;

        col.enabled = true;

        yield return new WaitForSeconds(0.25f);

        physicCol.enabled = false;

        col.enabled = false;

        yield return null;

        col.enabled = true;

        for (float i = 0; i < 0.2f; i += Time.deltaTime)
        {
            rb.velocity = (MovementController.mC.transform.position - transform.position).normalized * speed * (i * 5f);

            yield return null;
        }

        while (Vector2.Distance(transform.position, MovementController.mC.transform.position) > 1f)
        {
            rb.velocity = (MovementController.mC.transform.position - transform.position).normalized * speed;

            yield return null;
        }

        GainLife.gl.noSword = false;

        swordIdleAudio.Stop(gameObject);
        Destroy(gameObject);
    }
}
