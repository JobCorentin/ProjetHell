using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
    public class EnnemiWave : MonoBehaviour
    {
        public ArenaScript arena;
        public List<GameObject> waveEnnemi;
        bool ended = false;

        private void Start()
        {
            arena = gameObject.GetComponentInParent<ArenaScript>();
        }

        void Update()
        {
            if (waveEnnemi.Count == 0 && ended == false)
            {
                ended = true;
                arena.NextWave();
            }
        }
    }
}

