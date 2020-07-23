using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
    public class ArenaScript : MonoBehaviour
    {
        public CinemachineVirtualCamera arenaCamera;
        public GameObject arena;
        public List<GameObject> arenaWave;
        int waveCount;

        bool hasActivated = false;

        private void Start()
        {
            arenaCamera.Follow = GameObject.Find("Player").transform;
            arena.SetActive(false);
            foreach(GameObject wave in arenaWave)
            {
                wave.SetActive(false);
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Player" && hasActivated == false)
            {
                arena.SetActive(true);
                StartWave();
                hasActivated = true;
            }
        }

        private void Update()
        {
            if(arenaWave.Count <= 0)
            {
                arena.SetActive(false);
            }
        }

        public void StartWave()
        {
            waveCount = 0;
            arenaWave[waveCount].SetActive(true);

        }

        public void NextWave()
        {

            arenaWave[waveCount].SetActive(false);
            waveCount += 1;
            if (waveCount < arenaWave.Count)
            {
                arenaWave[waveCount].SetActive(true);
            }

        }
    }
}

