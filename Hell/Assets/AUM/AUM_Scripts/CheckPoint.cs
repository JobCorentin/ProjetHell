using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CheckPoint : MonoBehaviour
{

    [HideInInspector] public bool respawning = false;

    float existenceTimer = 0;

    private void Update()
    {
        existenceTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(HealthManager.hm.lastCheckPoint != this && existenceTimer >= 0.5f)
            {
                StartCoroutine(LitHearts());
            }

            HealthManager.hm.lastCheckPoint = this;
            PlayerData.pd.changePosition = true;
            PlayerData.pd.position = transform.position;
            if(gameObject.GetComponentInChildren<Animator>() != null)
            gameObject.GetComponentInChildren<Animator>().SetTrigger("lightUp");
        }
    }

    IEnumerator LitHearts()
    {
        /*MovementController.mC.stuned = true;

        HealthManager.hm.life = 0;

        MovementController.mC.animator.SetTrigger("Respawn");

        while (InputListener.iL.horizontalInput == 0)
        {
            yield return null;
        }

        MovementController.mC.animator.SetTrigger("GetUp");*/

        PostProcessBehaviour.ppb.HealProfile();
        for (int i = HealthManager.hm.life; i < HealthManager.hm.initialLife; i++)
        {
            yield return new WaitForSeconds(0.2f);

            HealthManager.hm.life++;
        }

        /*yield return new WaitForSeconds(1f);

        MovementController.mC.stuned = false;*/
    }

}
