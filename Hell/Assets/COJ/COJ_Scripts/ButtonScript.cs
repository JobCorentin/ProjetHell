using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{

    public static ButtonScript bs;

    private void Awake()
    {
        bs = this;
    }

    private void Update()
    {
        if (Input.GetKey("b"))
        {
            LoadScene(0);
        }
    }
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
