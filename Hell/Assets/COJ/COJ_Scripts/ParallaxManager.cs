using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    public static ParallaxManager pm;
    public GameObject[] parallaxList;
    // Start is called before the first frame update
    void Start()
    {
        pm = this;
        foreach (GameObject group in parallaxList)
        {
            if(group != parallaxList[0])
            {
                group.SetActive(false);
            }
        }
    }

    void ActivateGroup(int i)
    {
        parallaxList[i].SetActive(true);
        parallaxList[i - 1].SetActive(false);
    }

}
