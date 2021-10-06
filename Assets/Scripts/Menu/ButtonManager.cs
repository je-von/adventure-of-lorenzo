using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject mainPanel, optionPanel;
    public void ShowOptionsMenu()
    {
        mainPanel.SetActive(false);
        optionPanel.SetActive(true);

    }

    public void ShowMainMenu()
    {
        mainPanel.SetActive(true);
        optionPanel.SetActive(false);

    }

    public void ShowGameMenu()
    {
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        SceneManager.LoadScene(sceneName: "GameScene", LoadSceneMode.Single);
        yield return 0;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
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
