using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    GameObject m_ball;

    public float initBallSpeed;
    public float ballSpeedIncrement;

    public Vector3 initBallVector;
    public Vector3 initBallPosition;

    public int score;
    public int highScore;

    public bool isGameOn;
    public bool isGameOver;
    
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
    }
    void Start()
    {
        InitializeSettings();
        highScore = 0;
        UIManager.Instance.SetPanels(UIManager.Panels.MainMenu);

        Time.timeScale = 0f;
    }
    
    public void StartGame()
    {
        isGameOn = true;

        UIManager.Instance.SetPanels(UIManager.Panels.GamePanel);

        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        InitializeSettings();

        UIManager.Instance.SetPanels(UIManager.Panels.GamePanel);

        isGameOn = true;
        Time.timeScale = 1f;
    }

    void InitializeSettings() // 게임 매니저 내 변수 값 false 또는 0으로 초기화.
    {
        isGameOn = false;
        isGameOver = false;

        score = 0;
        UIManager.Instance.SetScoreText(score);
        UIManager.Instance.SetScoreTextAlpha(0f);
        UIManager.Instance.SetPanels(UIManager.Panels.SetAllPanelsFalse);

        m_ball.GetComponent<BallScript>().SetBallSpeed(initBallSpeed);
        m_ball.GetComponent<BallScript>().SetBallVector(initBallVector);
        m_ball.GetComponent<BallScript>().SetBallPosition(initBallPosition);
    }
    public void HandleGameOver()
    {
        isGameOn = false;
        isGameOver = true;

        if (score > highScore)
        {
            highScore = score;
        }

        UIManager.Instance.SetHighScoreText(highScore);
        UIManager.Instance.SetFinalScoreText(score);
        UIManager.Instance.SetPanels(UIManager.Panels.GameOver);

        Time.timeScale = 0f;
    }
}
