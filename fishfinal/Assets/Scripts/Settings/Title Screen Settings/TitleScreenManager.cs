using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject titleScreenPanel;
    [SerializeField] private string gameSceneName = "GameScene"; // your scene name

    void Start()
    {
        ShowTitleScreen();
    }

    public void ShowTitleScreen()
    {
        titleScreenPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnStartGame()
    {
        titleScreenPanel.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}