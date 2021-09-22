using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsManager : MonoBehaviour
{
    public AudioMixer mixer;
    public TMPro.TMP_Dropdown graphicDropdown, resolutionDropdown;
    Resolution[] resolutions;
    public void SetVolume(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializeResolutionDropdown();
        InitializeGraphicDropdown();
    }

    private void InitializeResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution(int index)
    {
        //Debug.Log("click: " +  index);
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void InitializeGraphicDropdown()
    {
        graphicDropdown.ClearOptions();
        List<string> options = new List<string>(QualitySettings.names);

        graphicDropdown.AddOptions(options);
        graphicDropdown.value = QualitySettings.GetQualityLevel(); ;
        graphicDropdown.RefreshShownValue();
    }
    public void SetGraphic(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
