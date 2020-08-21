using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Resolution[] resolutions;

    public Dropdown resolutionDropdown;

    public float Global;
    public AK.Wwise.RTPC globalWwiseVolume;

    public float SFX;
    public AK.Wwise.RTPC sfxWwiseVolume;

    public float music;
    public AK.Wwise.RTPC musicWwiseVolume;

    public GameObject optionMenu;

    public GameObject MenuFirstButton, optionsFirstButton, optionsClosedButton;


    private void Start()
    {
        SetFullscreen(true);
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        for(int i = 1;i < 3; i++)
        {
            string option = resolutions[resolutions.Length - i].width + " x " + resolutions[resolutions.Length - i].height;
            options.Add(option);

            /*if (resolutions[resolutions.Length - i].width == Screen.currentResolution.width && resolutions[resolutions.Length - i].height == Screen.currentResolution.height)
            {
                currentResolutuionIndex = i;
            }*/
        }

   

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        globalWwiseVolume.SetGlobalValue(Global);
        sfxWwiseVolume.SetGlobalValue(SFX);
        musicWwiseVolume.SetGlobalValue(music);
    }

    public void OpenOptions()
    {
        optionMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void CloseOptions()
    {
        optionMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsClosedButton);
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
        Resolution resolution = resolutions[resolutions.Length - resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
