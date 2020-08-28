using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTrigger : MonoBehaviour
{
    public List<GameObject> previousGroup;
    public bool isON = false;
    public ParallaxTrigger pt;
    private void Start()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && pt != null)
        {
            if(pt.isON == false)
            {
                isON = true;
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.SetActive(true);
                }

                if(previousGroup.Count > 0)
                {
                    foreach (GameObject pGroup in previousGroup)
                    {
                        for (int i = 0; i < pGroup.transform.childCount; i++)
                        {
                            pGroup.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }     
        }
        else if(collision.tag == "Player" && pt == null)
        {
            isON = true;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }

            if(previousGroup.Count > 0)
            {
                foreach (GameObject pGroup in previousGroup)
                {
                    for (int i = 0; i < pGroup.transform.childCount; i++)
                    {
                        pGroup.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
