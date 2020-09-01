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
        [HideInInspector] public List<EnnemiWave> arenaWaveControllers = new List<EnnemiWave>();
        int waveCount;

        bool hasActivated = false;
        bool finished = false;

        private void Start()
        {
            arenaCamera.Follow = GameObject.Find("Player").transform;
            arena.SetActive(false);
            foreach(GameObject wave in arenaWave)
            {
                wave.SetActive(false);

                arenaWaveControllers.Add(wave.GetComponent<EnnemiWave>());
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
            if (HealthManager.hm.lastCheckPoint != null)
                if (HealthManager.hm.lastCheckPoint.respawning)
                {
                    hasActivated = false;
                    arena.SetActive(false);
                    foreach (EnnemiWave wave in arenaWaveControllers)
                    {
                        foreach(EnnemiController ennemiController in wave.waveEnnemiControllers)
                        {
                            ennemiController.transform.position = ennemiController.initialPosition;
                            ennemiController.health = ennemiController.initialHealth;


                            ennemiController.sr.enabled = true;

                            ennemiController.playerDetected = false;


                            ennemiController.animator.SetBool("Respawning", true);
                            ennemiController.animator.SetBool("Spawning", true);
                            

                            ennemiController.dead = false;
                        }

                        wave.gameObject.SetActive(false);
                    }
                }

            if (waveCount >= arenaWave.Count && finished == false )
            {
                arena.SetActive(false);
            }
        }

        public void StartWave()
        {
            CameraShake.cs.cmArenaNoise = arenaCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            waveCount = 0;
            arenaWave[waveCount].SetActive(true);
            arenaWave[waveCount].GetComponent<EnnemiWave>().SpawnWave();

        }

        public void NextWave()
        {

            //arenaWave.Remove(arenaWave[waveCount]);
            waveCount += 1;
            if (waveCount < arenaWave.Count)
            {
                arenaWave[waveCount].SetActive(true);
                arenaWave[waveCount].GetComponent<EnnemiWave>().SpawnWave();
            }

        }
    }
}

