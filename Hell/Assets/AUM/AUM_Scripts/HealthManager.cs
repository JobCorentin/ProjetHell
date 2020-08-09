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




    [Space(10)]
    [Header("Sound")]
    public AK.Wwise.RTPC playerLifeGameSync; //Truc pour Link la variable life avec un filtre sonore (L'accouphène ici)
    public AK.Wwise.Event playerDamageAudio;
    public AK.Wwise.Event playerHealthConstantAudio; //Souffle et battement de coeur quand la vie est basse


    

    // Start is called before the first frame update
    void Start()
    {
        playerLifeGameSync.SetGlobalValue(life);
        playerHealthConstantAudio.Post(gameObject);

        hm = this;

        text = GameObject.Find("LifeText").GetComponent<TMPro.TextMeshProUGUI>();

        initialLife = life;
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

        if (life <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
