using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBox : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            ButtonScript.bs.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
