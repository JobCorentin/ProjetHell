using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cinemachine 
{
    public class BossWaveManager : MonoBehaviour
    {
        public GameObject arena;
        public EnnemiController boss;
        public int waveNum;
        [HideInInspector] public bool hasActivated;

        private void Update()
        {
            if (boss.playerDetected == true && hasActivated == false)
            {
                arena.GetComponent<ArenaScript>().StartWave();
                arena.GetComponent<ArenaScript>().arena.SetActive(true);
                hasActivated = true;
            }
            if (boss.health <= (boss.initialHealth / 3) * (waveNum-1))
            {
                arena.GetComponent<ArenaScript>().NextWave();
                waveNum--;
            }
        }
    }
}
