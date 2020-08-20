using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Resolution[] resolutions;

    public Dropdown resolutionDropdown;

    public float Global;
    public float SFX;
    public float music;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutuionIndex = 0;
        for(int i = 0;i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutuionIndex = i;
            }
        }

   

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutuionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        Debug.Log(volume);
    }

    public void SetVolumeGlobal(float volume)
    {
        Global = volume;
        Debug.Log(Global);
    }

    public void SetVolumeSFX(float volume)
    {
        SFX = volume;
        Debug.Log(SFX);
    }

    public void SetVolumeMusic(float volume)
    {
        music = volume;
        Debug.Log(music);
    }

    public void SetFullscreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
