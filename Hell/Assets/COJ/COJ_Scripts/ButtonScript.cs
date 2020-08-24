using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{

    public static ButtonScript bs;

    public AK.Wwise.Event mainMenuMusic;
    

    private void Awake()
    {
        bs = this;
    }


    public void Start()
    {
        mainMenuMusic.Post(gameObject);
    }

    private void Update()
    {
        if (Input.GetKey("b"))
        {
            LoadScene(0);
            mainMenuMusic.Stop(gameObject);
        }
    }
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
        mainMenuMusic.Stop(gameObject);
    }
}
