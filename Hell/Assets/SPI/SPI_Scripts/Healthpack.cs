using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Healthpack : PropsBehaviour
{
    public int heal;
    bool canHeal =true;


    public void HealPlayer()
    {
        if (canHeal && HealthManager.hm.life < HealthManager.hm.initialLife)
        {
                canHeal = false;
                HealthManager.hm.life += heal;
                PostProcessBehaviour.ppb.HealProfile();
        }
    }
}