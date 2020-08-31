﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public static HealthManager hm;

    public int life;
    [HideInInspector] public int initialLife;

    public float invicibiltyTime;
    float invicibilityTimer;

    [HideInInspector] public CheckPoint lastCheckPoint;

    Animator deathBackgroundAnimator;


    [Space(10)]
    [Header("Sound")]
    public AK.Wwise.RTPC lowPassDurationGameSync; //Truc pour Link la variable life avec un filtre sonore (L'accouphène ici)
    public AK.Wwise.Event playerDamageAudio;
    public AK.Wwise.Event playerHealthConstantAudio; //Souffle et battement de coeur quand la vie est basse
    public int lowPassDuration;
    int lowPassTimer;


    private void Awake()
    {
        hm = this;

        initialLife = 5;
    }

    // Start is called before the first frame update
    void Start()
    {
        MovementController.mC.animator.SetTrigger("Respawn");

        lowPassTimer = 0;
        lowPassDurationGameSync.SetGlobalValue(0);
        playerHealthConstantAudio.Stop(gameObject);
        playerHealthConstantAudio.Post(gameObject);

        deathBackgroundAnimator = GameObject.Find("DeathAnimation").GetComponent<Animator>();
    }



    private void FixedUpdate()
    {
        if (lowPassTimer > 0)
        {
            lowPassTimer--;
            lowPassDurationGameSync.SetGlobalValue(lowPassTimer);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(invicibilityTimer < invicibiltyTime)
        {
            invicibilityTimer += Time.deltaTime;
        }
    }

    public IEnumerator TakeDamage(int amount)
    {
        if (invicibilityTimer < invicibiltyTime)
            yield break;


        playerDamageAudio.Post(gameObject);
        lowPassTimer = lowPassDuration;

        life -= amount;
        FreezTimeManager.ftm.StartCoroutine(FreezTimeManager.ftm.FreezeTimeFor(0.1f, 0));
        invicibilityTimer = 0;
        if(life > 0)
        {
            PostProcessBehaviour.ppb.DamageProfile();
        }

        //BloodManager.bm.bloodNumb = 0;

        for(float i = 0.2f * amount; i > 0; i -= Time.deltaTime)
        {
            BaseSlashInstancier.bsi.sr.color = Color.red;

            yield return null;
        }

        BaseSlashInstancier.bsi.sr.color = Color.white;

        if (life <= 0)
        {
            PostProcessBehaviour.ppb.NullProfile();
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        MovementController.mC.animator.SetTrigger("Die");
        deathBackgroundAnimator.SetTrigger("Die");

        lowPassTimer = 0;
        lowPassDurationGameSync.SetGlobalValue(0);


        SoundManager.instance.haveKillAnEnnemi = false;
        SoundManager.instance.havePlayLevelTheme = false;
        SoundManager.instance.levelTheme.Stop(SoundManager.instance.gameObject);
        playerHealthConstantAudio.Stop(gameObject);


        float temp = MovementController.mC.rb.gravityScale;

        MovementController.mC.rb.gravityScale = 0;

        

        MovementController.mC.col.enabled = false;

        for(float i = 1.5f; i > 0; i -= Time.deltaTime)
        {
            MovementController.mC.rb.velocity = Vector2.zero;

            MovementController.mC.stuned = true;

            yield return null;
        }


        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
