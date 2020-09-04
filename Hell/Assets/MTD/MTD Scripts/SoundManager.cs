using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;


    public AK.Wwise.Event levelTheme;
    public AK.Wwise.Event bossTheme;
    public bool havePlayLevelTheme = false;
    public bool haveKillAnEnnemi = false;
    public bool havePlayedBossTheme = false;
    public bool isBossFight = false;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (!isBossFight)
        {
            if (havePlayLevelTheme == false)
            {
                if (haveKillAnEnnemi)
                {
                    levelTheme.Post(gameObject);
                    havePlayLevelTheme = true;
                }
            }
        }
    }
}
