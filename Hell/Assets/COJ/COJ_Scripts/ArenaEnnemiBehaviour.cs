using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
    public class ArenaEnnemiBehaviour : MonoBehaviour
    {
        public ArenaScript arena;
        public EnnemiController enemiStat;

        private void Start()
        {
            enemiStat = gameObject.GetComponent<EnnemiController>();
        }

        void Update()
        {
            if(enemiStat.health <= 0)
            {
                arena.arenaEnemi.Remove(gameObject);
            }
        }
    }
}

