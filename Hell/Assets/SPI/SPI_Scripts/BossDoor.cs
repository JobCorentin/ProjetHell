using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public static BossDoor bd;
    public List<SatueBossGuard> statues;
    GameObject player;
    public bool passed;

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
                    gameObject.GetComponent<Animator>().SetTrigger("SemiOpen");
                }
            }
            if (statuesToDestroy == statues.Count)
            {
                openDoor = true;
                gameObject.GetComponent<Animator>().SetTrigger("Open");
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
            if (openDoor == true && passed ==false)
            {
                passed = true;
                player = collision.gameObject;
                StartCoroutine(transitionDoor());
            }
        }
    }

    IEnumerator transitionDoor()
    {
        GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>().SetTrigger("FadeIn");
        SoundManager.instance.levelTheme.Stop(SoundManager.instance.gameObject);
        yield return new WaitForSeconds(1.2f);
        GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>().SetTrigger("FadeOut");
        player.transform.position = GameObject.FindGameObjectWithTag("BossTP").transform.position;
    }
}
