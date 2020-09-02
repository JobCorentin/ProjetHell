using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{

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
       /* resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        for(int i = 1;i < 3; i++)
        {
            string option = resolutions[resolutions.Length - i].width + " x " + resolutions[resolutions.Length - i].height;
            options.Add(option);

            if (resolutions[resolutions.Length - i].width == Screen.currentResolution.width && resolutions[resolutions.Length - i].height == Screen.currentResolution.height)
            {
                currentResolutuionIndex = i;
            }
        }



        resolutionDropdown.AddOptions(options);*/
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

    public void LoadLevel()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadCredit()
    {
        SceneManager.LoadScene(3);
    }

    public void Quit()
    {
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

    public void SetShake(bool isShaking)
    {
        OptionData.od.shaking = isShaking;
    }

    public void setResolution(int resolutionIndex)
    {
        if(resolutionIndex == 0)
        {
            Screen.SetResolution(1920, 1080, Screen.fullScreen);
        }
        else if (resolutionIndex == 1)
        {
            Screen.SetResolution(1600, 900, Screen.fullScreen);
        }
        else if (resolutionIndex == 2)
        {
            Screen.SetResolution(1440, 720, Screen.fullScreen);
        }
    }
}
