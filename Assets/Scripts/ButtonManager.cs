using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject mainPanel, optionPanel;
    public void ShowOptionsMenu()
    {
        //SceneManager.LoadScene(sceneName: "OptionScene");
        mainPanel.SetActive(false);
        optionPanel.SetActive(true);

    }

    public void ShowMainMenu()
    {
        //SceneManager.LoadScene(sceneName: "MainScene");
        mainPanel.SetActive(true);
        optionPanel.SetActive(false);

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Start()
    {
        ShowMainMenu();
    }
}
