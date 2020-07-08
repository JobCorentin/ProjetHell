using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehaviour : MonoBehaviour
{
    public int spikeDir;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 11)
        {
            HealthManager.hm.StartCoroutine(HealthManager.hm.TakeDamage(1));

            //Up
            if(spikeDir == 0)
            {
             MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(Vector2.up, MovementController.mC.rb, 0.3f, 2000, 0.1f));
            }
            //Down
            if (spikeDir == 1)
            {
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(Vector2.down, MovementController.mC.rb, 0.3f, 2000, 0.1f));
            }
            //Left
            if (spikeDir == 2)
            {
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(Vector2.left, MovementController.mC.rb, 0.3f, 2000, 0.1f));
            }
            //Right
            if (spikeDir == 3)
            {
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(Vector2.right, MovementController.mC.rb, 0.3f, 2000, 0.1f));
            }

        }
    }
}
