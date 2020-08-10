using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    public Rigidbody2D rb;

    public ParticleSystem dustPS;
    public ParticleSystem smallDustPS;
    float baseDustPSX;
    public MovementController mv;

    // Start is called before the first frame update
    void Start()
    {
        baseDustPSX = dustPS.velocityOverLifetime.x.constant;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(rb.velocity.x) < 0.1f)
            return;


        if (mv.pushedBack == false)
        {
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                //gameObject.transform.GetComponent<SpriteRenderer>().flipX = true;
            }

            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                //gameObject.transform.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

    }

    /*void LateUpdate()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[dustPS.particleCount];
        int count = dustPS.GetParticles(particles);
        for (int i = 0; i < count; i++)
        {
            float xVel = 0;

            if (InputListener.iL.horizontalInput < 0)
            {
                xVel = -baseDustPSX;
            }

            if (InputListener.iL.horizontalInput > 0)
            {
                xVel = baseDustPSX;
            }
            
            particles[i].velocity = new Vector3(xVel, particles[i].velocity.y, 0);
        }

        dustPS.SetParticles(particles, count);

        ParticleSystem.Particle[] particlesSmall = new ParticleSystem.Particle[smallDustPS.particleCount];
        int countSmall = smallDustPS.GetParticles(particlesSmall);
        for (int i = 0; i < count; i++)
        {
            float xVel = 0;

            if (rb.velocity.x < 0)
            {
                xVel = -baseDustPSX;
            }

            if (rb.velocity.x > 0)
            {
                xVel = baseDustPSX;
            }

            particlesSmall[i].velocity = new Vector3(xVel, particlesSmall[i].velocity.y, 0);
        }

        dustPS.SetParticles(particlesSmall, countSmall);
    }*/

    public void Fonction(string text)
    {
        Debug.Log(text);
    }
}
