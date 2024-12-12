using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject lifeIconPrefab;

    public Transform livesIconsParent;

    public Transform pauseView;
    public Transform endView;
    public TextMeshProUGUI endText;
    public TextMeshProUGUI scoreValueText;
    public TextMeshProUGUI highScoreValueText;

    void Start()
    {
        GameEvents.OnScoreChanged.AddListener(UpdateScore);
        GameEvents.OnHighScoreChanged.AddListener(UpdateHighScore);
        GameEvents.OnPlayerHit.AddListener(RemoveLifeIcon);

        int lives = Config.Instance.PlayerConfig.lives;
        for (int i = 0; i < lives; i++)
        {
            AddLifeIcon();
        }

        GameEvents.OnGameStateChanged.AddListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Gameplay:
                Resume();
                break;
            case GameState.Pause:
                Pause();
                break;
            case GameState.Win:
                EndGame(true);
                break;
            case GameState.Lose:
                EndGame(false);
                break;
        }
    }

    private void EndGame(bool win)
    {
        endView.gameObject.SetActive(true);
        endText.text = win ? "YOU WIN!" : "YOU LOSE!";
    }

    public void Pause()
    {
        pauseView.gameObject.SetActive(true);
    }

    public void Resume()
    {
        pauseView.gameObject.SetActive(false);
    }

    private void AddLifeIcon()
    {
        Instantiate(lifeIconPrefab, livesIconsParent);
    }

    private void RemoveLifeIcon()
    {
        if (livesIconsParent.childCount > 0)
        {
            Destroy(livesIconsParent.GetChild(0).gameObject);
        }

    }

    private void UpdateScore(int score)
    {
        scoreValueText.text = score.ToString();
    }

    private void UpdateHighScore(int highScore)
    {
        highScoreValueText.text = highScore.ToString();
    }
}
