using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTrigger2 : MonoBehaviour
{
    public GameObject previousGroup;
    public ParallaxTrigger pt;
    private void Start()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && pt.isON == false)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }

            if (previousGroup != null)
            {
                for (int i = 0; i < previousGroup.transform.childCount; i++)
                {
                    previousGroup.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
