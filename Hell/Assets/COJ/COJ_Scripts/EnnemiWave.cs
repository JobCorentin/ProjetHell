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

        public bool isBossWave;
        int waveCount;

        private void Start()
        {
            foreach(GameObject waveEnnem in waveEnnemi)
            {
                waveEnnemiControllers.Add(waveEnnem.GetComponent<EnnemiController>());
            }

            waveCount = waveEnnemi.Count;
            Debug.Log(waveCount);
            arena = gameObject.GetComponentInParent<ArenaScript>();
        }

        void Update()
        {
            if (isBossWave == false)
            {
                int ennemisDead = 0;

                foreach (EnnemiController waveEnnemiController in waveEnnemiControllers)
                {
                    if (waveEnnemiController.dead == true)
                    {
                        ennemisDead++;
                        Debug.Log(ennemisDead);
                    }                       
                }

                if (ennemisDead == waveCount && ended == false)
                {
                    ended = true;
                    StartCoroutine(delayBtwWave());
                    arena.NextWave();
                }
            }
        }

        public void SpawnWave()
        {
            foreach(GameObject waveEnnem in waveEnnemi)
            {
                waveEnnem.GetComponent<Animator>().SetBool("Spawning",true);
                StartCoroutine(waveEnnem.GetComponent<ArenaEnnemiBehaviour>().isSpawning());
            }
        }

        public IEnumerator delayBtwWave()
        {
            yield return new WaitForSeconds(1.5f);
            gameObject.SetActive(false);
        }
    }
}

