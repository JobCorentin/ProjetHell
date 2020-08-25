using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public static BossDoor bd;
    public List<SatueBossGuard> statues;
    public GameObject TpBossPose;
    public GameObject transitionFade;
    GameObject player;

    [HideInInspector] public int statuesToDestroy;

    public bool openDoor;

    public void Start()
    {
        bd = this;
    }
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
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
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
                player = collision.gameObject;
                StartCoroutine(transitionDoor());
            }
        }
    }

    IEnumerator transitionDoor()
    {
        transitionFade.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(1.2f);
        transitionFade.GetComponent<Animator>().SetTrigger("FadeOut");
        player.transform.position = TpBossPose.transform.position;
    }
}
