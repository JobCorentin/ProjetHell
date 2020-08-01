using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingTrigger : MonoBehaviour
{
    [TextArea]
    public string Instruction = ("Cet objet sert à lancer une musique quand le jeu se lance.");

    [Space(20)]
    [Header("Music")]
    public AudioClip musicClip = null;
    [Range(0f, 5f)] public float musicVolumeModificator = 1f;

    private void Start()
    {
        SoundManager.instance.PlayMusic(musicClip, musicVolumeModificator);
        Destroy(gameObject);
    }
}
