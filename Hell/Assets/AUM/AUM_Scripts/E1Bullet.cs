using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public EnnemiController ennemiLauncheFrom;

    public float existenceTime;
    float existenceTimer;

    bool reflected = false;


    [Space(10)]
    [Header("Sounds")]
    public AK.Wwise.Event projectilParryAudio;




    [HideInInspector] public float attackDirectionAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        existenceTimer += Time.deltaTime;

        if(existenceTimer >= existenceTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collision avec le layer Sol
        if(collision.gameObject.layer == 8)
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
            Vector2 v = new Vector2(1,1);
            Vector2 v2 = new Vector2(-1,1);
            if(collision.transform.position.x - transform.position.x >= 0)
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(v,MovementController.mC.rb, 0.3f,3300, 0.1f));
            else if(collision.transform.position.x - transform.position.x < 0)
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(v2, MovementController.mC.rb, 0.3f, 3300, 0.1f));



            Destroy(gameObject);
        }

        //Collision avec le layer Parry
        if(collision.gameObject.layer == 13)
        {
            projectilParryAudio.Post(gameObject);
            FXManager.fxm.fxInstancier(4, gameObject.transform, BaseSlashInstancier.bsi.attackDirectionAngle + Random.Range(-10, 10));

            //Parry.p.StopParry();

            reflected = true;

            FreezTimeManager.ftm.StartCoroutine(FreezTimeManager.ftm.FreezeTimeFor(0.2f, 0.5f));

            rb.velocity = (ennemiLauncheFrom.transform.position - transform.position).normalized * 40f;

            transform.rotation = Quaternion.Euler(0, 0, attackDirectionAngle + 180);
        }

        if (reflected == true)
        {
            if(collision.gameObject.tag == "Ennemi")
            {
                EnnemiController ec = collision.GetComponent<EnnemiController>();

                ec.StartCoroutine(ec.DamageDash((ennemiLauncheFrom.transform.position - transform.position).normalized, 0.1f, 500f, 1f));

                ec.StartCoroutine(ec.TakeDamage(1));

                Destroy(gameObject);
            }
        }
    }

    public void Orient(Vector2 currentAttackDirection)
    {
        if (currentAttackDirection.y > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            attackDirectionAngle = Vector2.Angle(transform.right, currentAttackDirection);
        }
        else
            attackDirectionAngle = Vector2.Angle(-transform.right, currentAttackDirection);

        transform.rotation = Quaternion.Euler(0, 0, attackDirectionAngle);
    }
}
