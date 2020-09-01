using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    void changeScene()
    {
        SceneManager.LoadScene(1);
    }
}
