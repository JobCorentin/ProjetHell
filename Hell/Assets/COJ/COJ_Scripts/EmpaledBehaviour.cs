using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpaledBehaviour : MonoBehaviour
{

    Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            MovementController.mC.canDoubleJump = true;
            BaseSlashInstancier.bsi.slashNumb = BaseSlashInstancier.bsi.slashNumbMax;
            BaseSlashInstancier.bsi.canGainHeight = true;

            if (BaseSlashInstancier.bsi.canBounce)
            {
                if (BaseSlashCollision.bsc.lastBounce != null)
                {
                    StopCoroutine(BaseSlashCollision.bsc.lastBounce);
                }

                BaseSlashCollision.bsc.lastBounce = MovementController.mC.StartCoroutine(BaseSlashCollision.bsc.Bounce());
            }

            animator.SetTrigger("hit");
        }
    }
}
