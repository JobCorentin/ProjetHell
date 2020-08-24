using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    [HideInInspector] public bool respawning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            HealthManager.hm.lastCheckPoint = this;
            PlayerData.pd.changePosition = true;
            PlayerData.pd.position = transform.position;
            if(gameObject.GetComponentInChildren<Animator>() != null)
            gameObject.GetComponentInChildren<Animator>().SetTrigger("lightUp");
        }
    }

}
