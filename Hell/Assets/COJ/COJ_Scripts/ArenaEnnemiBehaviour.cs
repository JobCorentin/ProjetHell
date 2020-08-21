﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
    public class ArenaEnnemiBehaviour : MonoBehaviour
    {
        public EnnemiWave wave;
        public EnnemiController enemiStat;
        Animator animator;

        public bool died;

        private void Awake()
        {
            enemiStat = gameObject.GetComponent<EnnemiController>();
            wave = gameObject.GetComponentInParent<EnnemiWave>();
            //enemiStat.animator.SetBool("Spawning",true);
            //StartCoroutine(isSpawning());
        }

        void Update()
        {
            if(enemiStat.health <= 0 && died==false)
            {
                died = true;
                wave.waveEnnemi.Remove(gameObject);
            }
        }

        public IEnumerator isSpawning()
        {
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            died = false;
            float spawnTime = Random.Range(0.5f, 1.5f);
            enemiStat.hasSpawn = false;

            yield return new WaitForSeconds(spawnTime);

            enemiStat.animator.SetBool("Respawning", false);

            enemiStat.animator.SetTrigger("hasSpawn");
        }
    }
}

