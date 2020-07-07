using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public static HealthManager hm;

    public int life;
    int initialLife;

    public float invicibiltyTime;
    float invicibilityTimer;

    public TMPro.TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        hm = this;

        text = GameObject.Find("LifeText").GetComponent<TMPro.TextMeshProUGUI>();

        initialLife = life;
    }

    // Update is called once per frame
    void Update()
    {
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

        life -= amount;
        invicibilityTimer = 0;

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
