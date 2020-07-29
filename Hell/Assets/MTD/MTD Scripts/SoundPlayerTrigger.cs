using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Matis Duperray
/// __
/// Joue un son quand le joueur trigger la zone
/// </summary>
public class SoundPlayerTrigger : MonoBehaviour
{
    [TextArea]
    public string Instruction = ("Cet objet sert à lancer un son quand le joueur passe dedans. Il n'est pas nécessaire de remplir les clip si vous ne souhaitez pas qu'ils se jouent (Ex : je veux un sfx mais pas une musique). Les modificateur de Volumes et de pitch servent à ajuster le volume en fonction des autres sons.");


    [Space(20)]
    public bool destoyAfterUse;

    [Space(10)]
    [Header("Music")]
    public AudioClip musicClip = null;
    [Range(0f, 5f)] public float musicVolumeModificator = 1f;

    [Space(10)]
    [Header("SFX")]
    public AudioClip sfxClip = null;
    [Range(0f, 5f)] public float sfxVolumeModificator = 1f;
    [Range(0f, 5f)] public float sfxPitchModificator = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (musicClip != null)
            {
                SoundManager.instance.PlayMusic(musicClip, musicVolumeModificator);
            }
            if (sfxClip != null)
            {
                SoundManager.instance.PlaySfx(musicClip, sfxVolumeModificator, sfxPitchModificator);
            }

            if (destoyAfterUse)
            {
                Destroy(gameObject);
            }
        }
    }
}
