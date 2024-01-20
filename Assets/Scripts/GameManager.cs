using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    GameObject m_ball;
    public GameObject player;
    GameObject m_ballSpawnPoint;

    public float initBallSpeed;
    public float ballSpeedIncrement;

    public Vector3 initBallVector;
    public Vector3 initBallPosition;

    public int score;
    public int highScore;
    public enum GameStates
    {
        MainMenuState,
        GameState,
        GameOverState,
        OptionState
    }

    public GameStates gameState;

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
        highScore = 0;

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
    public void RestartGame()
    {
        InitializeSettings();

        gameState = GameStates.GameState;

        UIManager.Instance.HideCursor();
        UIManager.Instance.SetPanels(UIManager.Panels.GamePanel);

        Time.timeScale = 1f;
    }

    void InitializeSettings() // 게임 매니저 내 변수 값 false 또는 0으로 초기화.
    {
        score = 0;

        UIManager.Instance.SetScoreText(score);
        UIManager.Instance.SetScoreTextAlpha(0f);
        UIManager.Instance.SetPanels(UIManager.Panels.SetAllPanelsFalse);

        m_ball.GetComponent<BallScript>().SetBallSpeed(initBallSpeed);

        m_ball.GetComponent<BallScript>().SetBallVector(initBallVector);
        m_ball.GetComponent<BallScript>().RotateBallVector(Random.Range(0f, 360f));

        m_ball.GetComponent<BallScript>().SetBallPosition(m_ballSpawnPoint.transform.position);
    }
    public void HandleGameOver()
    {
        if (score > highScore)
        {
            highScore = score;
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
