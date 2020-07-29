﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthpack : PropsBehaviour
{
    public int heal;
    bool canHeal =true;

    [HideInInspector] public HealthManager player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            player = collision.GetComponentInParent<HealthManager>();
        }
    }
    private void Update()
    {
        if (isDestroyed)
            HealPlayer();
    }

    public void HealPlayer()
    {
        if (canHeal)
        {
            canHeal = false;
            player.life += heal;
        }
    }
}