using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// by Tristan Ledieu
/// Remastered by Matis Duperray
/// </summary>
public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    // = = = [ VARIABLES DEFINITION ] = = =
    #region Variables Definition

    [Space(10)]
    [Header("Global")]
    [Range(0f, 1f)] public float globalDefaultVolume = 1f;

    [Space(10)]
    [Header("Musics")]
    [Range(0f, 1f)] public float musicDefaultVolume = 1f;

    [Space(10)]
    [Header("SFX")]
    [Range(0f, 1f)] public float sfxDefaultVolume = 1f;

    [Space(10)]
    [Header("References")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    #endregion
    // = = =

    // = = = [ MONOBEHAVIOR METHODS ] = = =
    #region Monobehavior Methods

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion
    // = = =

    // = = = [ CLASS METHODS ] = = =
    #region Class Methods


    /// <summary>
    /// Start playing a given music.
    /// </summary>
    public void PlayMusic(AudioClip music, float volume)
    {
        musicSource.clip = music;
        musicSource.volume = musicDefaultVolume * globalDefaultVolume * volume;

        musicSource.Play();

        return;
    }

    /// <summary>
    /// Plays a given sfx. Specific volume and pitch can be specified in parameters.
    /// </summary>
    public void PlaySfx(AudioClip sfx, float volume, float pitch)
    {
        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(sfx, sfxDefaultVolume * globalDefaultVolume * volume);

        sfxSource.pitch = 1;

        return;
    }



    /// <summary>
    /// Stop the actual playing music.
    /// </summary>
    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    /// <summary>
    /// Stop the actual playing SFX.
    /// </summary>
    public void StopSFX()
    {
        if (sfxSource.isPlaying)
        {
            sfxSource.Stop();
        }
    }

    /// <summary>
    /// Pause the actual playing music.
    /// </summary>
    public void PauseMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    /// <summary>
    /// Unpause an actual paused music.
    /// </summary>
    public void UnPauseMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.UnPause();
        }
    }

    #endregion
    // = = =

}
