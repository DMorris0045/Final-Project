using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 10f;
    public bool timerIsRunning;

    public TextMeshProUGUI timeText;

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (!timerIsRunning)
        {
            return;
        }

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(timeRemaining, 0f);
            DisplayTime(timeRemaining);
        }
        else
        {
            PlayerSurvived();
        }
    }

    public void StartTimer()
    {
        timerIsRunning = true;
        DisplayTime(timeRemaining);
    }

    public void StopTimer()
    {
        timerIsRunning= false;
    }

    private void PlayerSurvived()
    {
        timerIsRunning = false;
        timeRemaining = 0f;

        DisplayTime(timeRemaining);

        Debug.Log("Congrats! You survived!");
        GameUI.SetWinResult();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void DisplayTime(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60f);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60f);

        if (timeText != null)
        {
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}