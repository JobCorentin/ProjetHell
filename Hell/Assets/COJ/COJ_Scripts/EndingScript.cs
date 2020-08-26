using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScript : MonoBehaviour
{
    EnnemiController en;
    bool hasAct;

    void Start()
    {
        en = gameObject.GetComponentInParent<EnnemiController>();
    }

    void Update()
    {
        if(en.health <= 0 && hasAct == false)
        {
            hasAct = true;
            SceneManager.LoadScene(2);
        }
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
    }
}
