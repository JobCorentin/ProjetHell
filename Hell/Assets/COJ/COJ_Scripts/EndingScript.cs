using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScript : MonoBehaviour
{
    public EnnemiController en;
    bool hasAct;

    UnityEngine.UI.Slider barBossSlider;

    void Start()
    {
        barBossSlider = GameObject.Find("BossSlider").GetComponent<UnityEngine.UI.Slider>();

        barBossSlider.transform.parent.gameObject.SetActive(false);
    }

    void Update()
    {
        if(en.playerDetected == true)
        {
            barBossSlider.transform.parent.gameObject.SetActive(true);
            barBossSlider.value = (float)en.health / (float)en.initialHealth;
        }
    }

    public IEnumerator Load()
    {
        GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>().SetTrigger("FadeIn");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(2);
    }
}
