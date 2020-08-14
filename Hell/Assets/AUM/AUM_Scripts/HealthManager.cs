using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public static HealthManager hm;

    public int life;
    [HideInInspector] public int initialLife;

    public float invicibiltyTime;
    float invicibilityTimer;

    TMPro.TextMeshProUGUI text;

    [HideInInspector] public CheckPoint lastCheckPoint;

    Animator fadeAnimator;


    [Space(10)]
    [Header("Sound")]
    public AK.Wwise.RTPC playerLifeGameSync; //Truc pour Link la variable life avec un filtre sonore (L'accouphène ici)
    public AK.Wwise.Event playerDamageAudio;
    public AK.Wwise.Event playerHealthConstantAudio; //Souffle et battement de coeur quand la vie est basse


    

    // Start is called before the first frame update
    void Start()
    {
        MovementController.mC.animator.SetTrigger("Respawn");

        playerLifeGameSync.SetGlobalValue(life);
        playerHealthConstantAudio.Post(gameObject);

        hm = this;

        text = GameObject.Find("LifeText").GetComponent<TMPro.TextMeshProUGUI>();

        initialLife = life;

        fadeAnimator = GameObject.Find("Fade").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerLifeGameSync.SetGlobalValue(life);

        text.text = "Life : " + life;

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

        life -= amount;
        FreezTimeManager.ftm.StartCoroutine(FreezTimeManager.ftm.FreezeTimeFor(0.1f, 0));
        invicibilityTimer = 0;

        //BloodManager.bm.bloodNumb = 0;

        for(float i = 0.2f * amount; i > 0; i -= Time.deltaTime)
        {
            BaseSlashInstancier.bsi.sr.color = Color.red;

            yield return null;
        }

        BaseSlashInstancier.bsi.sr.color = Color.white;

        if (life <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        MovementController.mC.animator.SetTrigger("Die");

        float temp = MovementController.mC.rb.gravityScale;

        MovementController.mC.rb.gravityScale = 0;

        

        MovementController.mC.col.enabled = false;

        for(float i = 1.5f; i > 0; i -= Time.deltaTime)
        {
            MovementController.mC.rb.velocity = Vector2.zero;

            MovementController.mC.stuned = true;

            yield return null;
        }

        fadeAnimator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(1.5f);

        MovementController.mC.transform.position = lastCheckPoint.transform.position;

        life = initialLife;

        lastCheckPoint.respawning = true;

        MovementController.mC.animator.SetTrigger("Respawn");

        MovementController.mC.col.enabled = true;

        MovementController.mC.rb.gravityScale = temp;

        fadeAnimator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1f);

        lastCheckPoint.respawning = false;

        MovementController.mC.stuned = false;

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
