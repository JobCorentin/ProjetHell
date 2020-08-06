﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomController : MonoBehaviour
{
    [HideInInspector] public Vector2 target;

    [HideInInspector] public float speed;

    public Rigidbody2D rb;

    public SpriteRenderer sr;

    [HideInInspector] public bool touchedEnnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(touchedEnnemy == true)
        {
            sr.color = Color.blue;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ennemi")
        {
            FXManager.fxm.fxInstancier(2, collision.transform, BaseSlashInstancier.bsi.attackDirectionAngle + Random.Range(-10, 10));
            FXManager.fxm.fxInstancier(4, collision.transform, 0);

            EnnemiController ec = collision.GetComponent<EnnemiController>();
            CameraShake.cs.WeakShake();

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

                etc.StopLaunchBullet();
            }
            if (ec.type == 2)
            {
                EnnemiThreeBehavior erc = ec.GetComponent<EnnemiThreeBehavior>();

                erc.StopAttack();
            }

        }
    }

    public IEnumerator GoToTargetThenPlayer()
    {
        GainLife.gl.noSword = true;

        while(Vector2.Distance(transform.position, target) > 10f)
        {
            rb.velocity = (target -(Vector2)transform.position).normalized * speed;

            yield return null;
        }

        for(float i = 0.2f; i > 0; i -= Time.deltaTime)
        {
            rb.velocity = (target - (Vector2)transform.position).normalized * speed * (i * 5f);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

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

        if(touchedEnnemy == true)
        {
            if(HealthManager.hm.life < HealthManager.hm.initialLife)
            {
                HealthManager.hm.life++;
            }
        }

        GainLife.gl.noSword = false;

        Destroy(gameObject);
    }
}
