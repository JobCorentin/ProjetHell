using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpaledBehaviour : MonoBehaviour
{

    Animator animator;
    public bool isCharger;

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
            if (isCharger)
            {
                FXManager.fxm.fxInstancier(2, collision.transform, BaseSlashInstancier.bsi.attackDirectionAngle);
                if (BloodManager.bm.bloodNumb < BloodManager.bm.bloodNumbMax)
                    BloodManager.bm.bloodNumb += 1;
            }
            animator.SetTrigger("hit");
        }
    }
}
