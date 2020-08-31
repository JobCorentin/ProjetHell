using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityEngine.Rendering
{
    public class PostProcessBehaviour : MonoBehaviour
    {
        public static PostProcessBehaviour ppb;

        public Volume v;
        public VolumeProfile main,heal,damage, nullProfile;
        public bool activate;

        void Start()
        {
            ppb = this;
        }

        public void HealProfile()
        {
            v.profile = heal;
            gameObject.GetComponent<Animator>().SetTrigger("heal");
        }

        public void DamageProfile()
        {
            v.profile = damage;
            gameObject.GetComponent<Animator>().SetTrigger("damage");
        }

        public void NullProfile()
        {
            v.profile = nullProfile;
        }

        public void ResetProfile()
        {
            v.profile = main;
        }
    }
}