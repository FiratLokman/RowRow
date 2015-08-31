using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{


    public GameObject StartScreen;
    public GameObject GameOverScreen;
    public GameObject GameObjects;
    public GameObject SettingsScreen;
    public static bool IsGameStarted = false;
    public static bool CanMove = true;
    public static bool GameOver = false;
    public static bool IsPaused = false;
    public static int Score;
    public Spawner spawner;

    public Text FinalScore;
    public Text CurrentScore;
    public Text HighScore;

    public Button MuteButton;
    public Button UnMuteButton;

    // Use this for initialization
    void Start()
    {
        GameOverScreen.SetActive(false);
        GameObjects.SetActive(false);
        SettingsScreen.SetActive(false);

        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver)
        {
            SettingsScreen.SetActive(false);
            IsGameStarted = false;
            StartScreen.SetActive(false);
            GameOverScreen.SetActive(true);
            FinalScore.text = Score.ToString();

            if (Score > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", Score);
            }

            HighScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        }

        CurrentScore.text = Score.ToString();
    }

    public void GameStart()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        IsGameStarted = true;
        SettingsScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        StartScreen.SetActive(false);
        GameObjects.SetActive(true);
        spawner.spawnNext();
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0.01f;
        SettingsScreen.SetActive(true);
        GameOverScreen.SetActive(false);
        StartScreen.SetActive(false);
        GameObjects.SetActive(false);
    }

    public void Continue()
    {
        IsPaused = false;
        Time.timeScale = 1;
        SettingsScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        StartScreen.SetActive(false);
        GameObjects.SetActive(true);
    }

    public void Replay()
    {
        Grid.ClearAll();
        SettingsScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        StartScreen.SetActive(false);
        GameObjects.SetActive(true);
        CanMove = true;
        IsGameStarted = true;
        GameOver = false;
        spawner.spawnNext();
        Score = 0;
    }

    public void ReturnHome()
    {
        Grid.ClearAll();
        SettingsScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        StartScreen.SetActive(true);
        GameObjects.SetActive(false);
        CanMove = true;
        IsGameStarted = false;
        GameOver = false;
    }

    public void Settings()
    {
        Grid.ClearAll();
        SettingsScreen.SetActive(true);
        GameOverScreen.SetActive(false);
        StartScreen.SetActive(false);
        GameObjects.SetActive(false);
        CanMove = true;
        IsGameStarted = false;
        GameOver = false;
    }

    //public void Mute()
    //{
    //    foreach (AudioSource Source in FindObjectsOfType<AudioSource>())
    //    {
    //        Source.mute = true;
    //    }
    //}

    //public void UnMute()
    //{
    //    foreach (AudioSource Source in FindObjectsOfType<AudioSource>())
    //    {
    //        Source.mute = false;
    //    }
    //}
}
