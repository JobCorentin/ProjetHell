using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
    public class EnnemiWave : MonoBehaviour
    {
        public ArenaScript arena;
        public List<GameObject> waveEnnemi;
        [HideInInspector] public List<EnnemiController> waveEnnemiControllers = new List<EnnemiController>();
        bool ended = false;

        private void Start()
        {
            foreach(GameObject waveEnnem in waveEnnemi)
            {
                waveEnnemiControllers.Add(waveEnnem.GetComponent<EnnemiController>());
            }

            arena = gameObject.GetComponentInParent<ArenaScript>();
        }

        void Update()
        {
            int ennemisDead = 0;

            foreach(EnnemiController waveEnnemiController in waveEnnemiControllers)
            {
                if (waveEnnemiController.dead == true)
                    ennemisDead++;
            }

            if (ennemisDead == waveEnnemi.Count && ended == false)
            {
                ended = true;
                gameObject.SetActive(false);
                arena.NextWave();
            }
        }
    }
}

