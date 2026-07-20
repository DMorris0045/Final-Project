using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject winText;
    public GameObject loseText;

    private static bool showWinOnTitle;
    private static bool showLoseOnTitle;

    private void Start()
    {
        Time.timeScale = 1f;

        if (winText != null)
        {
            winText.SetActive(showWinOnTitle);
        }

        if (loseText != null)
        {
            loseText.SetActive(showLoseOnTitle);
        }

        showWinOnTitle = false;
        showLoseOnTitle = false;
    }

    public static void SetWinResult()
    {
        showWinOnTitle = true;
        showLoseOnTitle = false;
    }

    public static void SetLoseResult()
    {
        showWinOnTitle = false;
        showLoseOnTitle = true;
    }

    public void PlayGame()
    {
        showWinOnTitle = false;
        showLoseOnTitle = false;

        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}