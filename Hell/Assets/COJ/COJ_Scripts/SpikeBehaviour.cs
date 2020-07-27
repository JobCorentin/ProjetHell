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
            Vector2 diagonaleDroite = new Vector2(1, 1);
            Vector2 diagonaleGauche = new Vector2(-1, 1);
            //Renvoyer vers la droite
            if(collision.transform.position.x - transform.position.x >= 0) 
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(diagonaleDroite, MovementController.mC.rb, 0.3f, 3300, 0.1f));
            //Renvoyer vers la gauche
            else if(collision.transform.position.x - transform.position.x < 0)
                MovementController.mC.StartCoroutine(MovementController.mC.MiniDash(diagonaleGauche, MovementController.mC.rb, 0.3f, 3300, 0.1f));

        }
    }
}
