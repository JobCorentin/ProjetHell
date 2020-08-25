using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public List<SatueBossGuard> statues;
    public GameObject TpBossPose;

    [HideInInspector] public int statuesToDestroy;

    public bool openDoor;
    private void Update()
    {
        if(openDoor == false)
        {
            foreach (SatueBossGuard stat in statues)
            {
                if (stat.destroyed == true)
                {
                    statuesToDestroy++;
                }
            }
            if (statuesToDestroy == statues.Count)
            {
                openDoor = true;
            }
            else
            {
                openDoor = false;
                statuesToDestroy = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (openDoor == true)
            {
                collision.transform.position = TpBossPose.transform.position;
            }
        }
    }
}
