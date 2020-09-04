
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionInitiator : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        if(PlayerData.pd != null)
        if(PlayerData.pd.changePosition == true)
        {
            MovementController.mC.transform.position = PlayerData.pd.position;
        }

        StartCoroutine(LitHearts());
    }

    IEnumerator LitHearts()
    {
        MovementController.mC.stuned = true;

        HealthManager.hm.life = 0;

        MovementController.mC.animator.SetTrigger("Respawn");

        while(InputListener.iL.horizontalInput == 0)
        {
            yield return null;
        }

        MovementController.mC.animator.SetTrigger("GetUp");

        for(int i = 0; i < HealthManager.hm.initialLife; i++)
        {
            yield return new WaitForSeconds(0.1f);

            HealthManager.hm.life++;
        }

        yield return new WaitForSeconds(1f);

        MovementController.mC.stuned = false;

        Destroy(gameObject);
    }
}
