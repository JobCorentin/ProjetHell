using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
    public class ArenaEnnemiBehaviour : MonoBehaviour
    {
        public EnnemiWave wave;
        public EnnemiController enemiStat;

        private void Start()
        {
            enemiStat = gameObject.GetComponent<EnnemiController>();
            wave = gameObject.GetComponentInParent<EnnemiWave>();
        }

        void Update()
        {
            if(enemiStat.health <= 0)
            {
                wave.waveEnnemi.Remove(gameObject);
            }
        }
    }
}

