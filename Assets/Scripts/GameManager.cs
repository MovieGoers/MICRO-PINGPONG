using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public float initBallSpeed;
    public float ballSpeedIncrement;

    public Vector3 initBallVector;
    public Vector3 initBallPosition;

    public int score;
    public int highScore;

    public GameObject player;

    public GameObject wallBottom;
    public GameObject wallLeft;
    public GameObject wallRight;
    public GameObject wallTop;
    public GameObject wallBack;

    private GameObject m_ball;
    private GameObject m_ballSpawnPoint;

    public enum GameStates
    {
        MainMenuState,
        GameState,
        GameOverState,
        OptionState,
        CreditsState
    }

    public GameStates gameState;

    public bool DebugMode;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        m_ball = GameObject.Find("Ball");
        player = GameObject.Find("Player");
        m_ballSpawnPoint = GameObject.Find("Ball Spawn Point");
    }
    void Start()
    {
        InitializeSettings();
        highScore = -1;

        gameState = GameStates.MainMenuState;

        UIManager.Instance.DisplayCursor();
        UIManager.Instance.SetPanels(UIManager.Panels.MainMenu);

        Time.timeScale = 0f;

        AudioManager.Instance.Play("Music1");

    }
    
    public void ShowStartMenu()
    {
        gameState = GameStates.MainMenuState;

        UIManager.Instance.DisplayCursor();
        UIManager.Instance.SetPanels(UIManager.Panels.MainMenu);

        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        gameState = GameStates.GameState;

        UIManager.Instance.HideCursor();
        UIManager.Instance.SetPanels(UIManager.Panels.GamePanel);

        Time.timeScale = 1f;
    }

    public void ShowOptions()
    {
        gameState = GameStates.OptionState;

        UIManager.Instance.DisplayCursor();
        UIManager.Instance.SetPanels(UIManager.Panels.Options);

        Time.timeScale = 0f;
    }

    public void ShowCredits()
    {
        gameState = GameStates.CreditsState;

        UIManager.Instance.DisplayCursor();
        UIManager.Instance.SetPanels(UIManager.Panels.Credits);

        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        InitializeSettings();

        gameState = GameStates.GameState;

        UIManager.Instance.HideCursor();
        UIManager.Instance.SetPanels(UIManager.Panels.GamePanel);

        Time.timeScale = 1f;
    }

    void InitializeSettings() // 게임 초깃값 설정.
    {
        score = 0;

        UIManager.Instance.SetScoreText(score);
        UIManager.Instance.SetScoreTextAlpha(0f);
        UIManager.Instance.SetPanels(UIManager.Panels.SetAllPanelsFalse);
        UIManager.Instance.itemEffectText.SetActive(false);

        PlayerScript.Instance.gameObject.transform.localScale = PlayerScript.Instance.originalPlayerScale;
        PlayerScript.Instance.addingScore = 1;
        PlayerScript.Instance.tempScore = 0;

        BallScript.Instance.SetBallSpeed(initBallSpeed);
        BallScript.Instance.SetBallVector(initBallVector);
        BallScript.Instance.RotateBallVector(Random.Range(0f, 360f));
        BallScript.Instance.SetBallPosition(m_ballSpawnPoint.transform.position);
        BallScript.Instance.SetBallSize(BallScript.Instance.originalBallScale);

        ItemManager.Instance.SpawnItem(ItemManager.Instance.item_BallSizeGrow);
        ItemManager.Instance.SpawnItem(ItemManager.Instance.item_PlayerSizeGrow);
        ItemManager.Instance.SpawnItem(ItemManager.Instance.item_ScoreDouble);
        StopCoroutine("PlayItemEffectTextAnimation");

        ItemManager.Instance.DeactivateItemEffect();
    }
    public void HandleGameOver()
    {
        if (score > highScore)
        {
            highScore = score;
            LeaderboardsManager.Instance.HandleLeaderboard(score);
        }

        gameState = GameStates.GameOverState;

        UIManager.Instance.DisplayCursor();
        UIManager.Instance.SetHighScoreText(highScore);
        UIManager.Instance.SetFinalScoreText(score);
        UIManager.Instance.SetPanels(UIManager.Panels.GameOver);

        Time.timeScale = 0f;

        AudioManager.Instance.Play("GameOver");
    }

    public IEnumerator SlowTime(float timeScale, float duration)
    {
        yield return null;
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
}
