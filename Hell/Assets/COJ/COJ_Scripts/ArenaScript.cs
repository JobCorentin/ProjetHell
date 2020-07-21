using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
    public class ArenaScript : MonoBehaviour
    {
        public CinemachineVirtualCamera arenaCamera;
        public GameObject arena;
        public List<GameObject> arenaEnemi;

        bool hasActivated = false;

        private void Start()
        {
            arenaCamera.Follow = GameObject.Find("Player").transform;
            arena.SetActive(false);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Player" && hasActivated == false)
            {
                arena.SetActive(true);
                hasActivated = true;
            }
        }

        private void FixedUpdate()
        {
            if(arenaEnemi.Count <= 0)
            {
                arena.SetActive(false);
            }
        }
    }
}

