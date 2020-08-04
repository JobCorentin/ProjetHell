using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiSlashCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collision avec le layer Player
        if (collision.gameObject.layer == 11)
        {
            HealthManager.hm.StartCoroutine(HealthManager.hm.TakeDamage(1));
            Vector2 v = new Vector2(1, 0.5f);
            Vector2 v2 = new Vector2(-1, 0.5f);
            if (collision.transform.position.x - transform.position.x >= 0)
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(v, MovementController.mC.rb, 0.3f, 4700, 0.1f));
            else if (collision.transform.position.x - transform.position.x < 0)
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(v2, MovementController.mC.rb, 0.3f, 4700, 0.1f));

        }
    }
}
