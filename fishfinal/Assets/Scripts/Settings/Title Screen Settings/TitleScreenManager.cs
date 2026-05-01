using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject titleScreenPanel;

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
    InstructionsManager.Instance.Show();
    }


    public void OnExitGame()
    {
        Application.Quit();
    }
}