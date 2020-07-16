using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
    public class ArenaScript : MonoBehaviour
    {
        public CinemachineConfiner confiner;
        public GameObject arena;
        public GameObject confinerCollider;
        public List<GameObject> arenaEnemi;

        bool hasActivated = false;

        private void Start()
        {
            arena.SetActive(false);
            confiner = GameObject.Find("CM vcam1").GetComponent<CinemachineConfiner>();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Player" && hasActivated == false)
            {
                arena.SetActive(true);
                confiner.m_BoundingShape2D = confinerCollider.GetComponent<PolygonCollider2D>();
                hasActivated = true;
            }
        }

        private void FixedUpdate()
        {
            if(arenaEnemi.Count <= 0)
            {
                confiner.m_BoundingShape2D = null;
                arena.SetActive(false);
            }
        }
    }
}

