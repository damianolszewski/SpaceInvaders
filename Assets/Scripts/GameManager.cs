using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum GameState
{
    Menu,
    Start,
    Gameplay,
    Pause,
    Win,
    Lose
}

public class GameManager : MonoBehaviour
{
    public InputActionReference pauseActionReference;

    public Config Config { get; private set; }

    public int Score { get; private set; }

    private GameState gameState;

    private void Start()
    {
        Time.timeScale = 1;
        GameEvents.OnGameStateChanged.AddListener(OnGameStateChanged);
        GameEvents.OnEnemyKilled.AddListener(OnEnemyKilled);
        pauseActionReference.action.performed += _ => TogglePause();
        Invoke(nameof(GameStart), 1);
    }

    private void TogglePause()
    {
        if (gameState == GameState.Gameplay)
        {
            GameEvents.OnGameStateChanged.Invoke(GameState.Pause);
        }
        else if (gameState == GameState.Pause)
        {
            GameEvents.OnGameStateChanged.Invoke(GameState.Gameplay);
        }
    }

    private void OnGameStateChanged(GameState state)
    {
        gameState = state;
        switch (state)
        {
            case GameState.Start:
            case GameState.Gameplay:
                Time.timeScale = 1;
                break;
            case GameState.Pause:
            case GameState.Win:
            case GameState.Lose:
                Time.timeScale = 0;
                break;
        }
    }

    private void OnEnemyKilled(EnemyLogic enemy)
    {
        AddScore(enemy.GetScoreValue());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        GameEvents.OnGameStateChanged.Invoke(GameState.Pause);
    }

    public void Resume()
    {
        GameEvents.OnGameStateChanged.Invoke(GameState.Gameplay);
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    private void GameStart()
    {
        SetScore(0);
        GameEvents.OnHighScoreChanged.Invoke(PlayerPrefs.GetInt("HighScore", 0));
        GameEvents.OnGameStateChanged.Invoke(GameState.Start);
    }

    public void SetScore(int score)
    {
        Score = score;
        GameEvents.OnScoreChanged.Invoke(Score);
    }

    public void AddScore(int amount)
    {
        Score += amount;
        if(Score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", Score);
            GameEvents.OnHighScoreChanged.Invoke(Score);
        }
        GameEvents.OnScoreChanged.Invoke(Score);
    }
}
