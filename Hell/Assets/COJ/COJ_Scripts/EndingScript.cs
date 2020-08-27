using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScript : MonoBehaviour
{
    public EnnemiController en;
    bool hasAct;

    void Start()
    {
        
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
