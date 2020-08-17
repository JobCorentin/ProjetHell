using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusicTest : MonoBehaviour
{
    public AK.Wwise.Event myMusic;


    // Start is called before the first frame update
    void Start()
    {
        myMusic.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
