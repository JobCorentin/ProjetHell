using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public EnnemiOneController ennemiLauncheFrom;

    public float existenceTime;
    float existenceTimer;

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

        //Collision avec le layer Player
        if (collision.gameObject.layer == 11)
        {
            HealthManager.hm.StartCoroutine(HealthManager.hm.TakeDamage(1));
            MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(rb.velocity ,MovementController.mC.rb, 0.3f, 5000, 0.1f));

            Destroy(gameObject);
        }
    }
}
