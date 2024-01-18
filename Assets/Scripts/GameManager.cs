using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    GameObject m_ball;
    GameObject m_mainMenu;
    GameObject m_GameOver;
    GameObject m_GamePanel;

    public GameObject scoreText;
    public GameObject finalScoreText;
    public GameObject highScoreText;

    public float initBallSpeed;
    public float ballSpeedIncrement;

    public Vector3 initBallVector;
    public Vector3 initBallPosition;

    public int score;
    public int highScore;

    public bool isGameOn;
    public bool isGameOver;

    enum Panels
    {
        MainMenu,
        GameOver,
        GamePanel
    }
    
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

        scoreText = GameObject.Find("Canvas/Game Panel/Score");
        finalScoreText = GameObject.Find("Canvas/Game Over/Final Score");
        highScoreText = GameObject.Find("Canvas/Game Over/High Score");

        m_mainMenu = GameObject.Find("Canvas/Main Menu");
        m_GameOver = GameObject.Find("Canvas/Game Over");
        m_GamePanel = GameObject.Find("Canvas/Game Panel");
    }
    void Start()
    {
        InitializeSettings();
        highScore = 0;
        m_mainMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    
    public void StartGame()
    {
        isGameOn = true;

        SetPanels(Panels.GamePanel);

        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        InitializeSettings();

        SetPanels(Panels.GamePanel);

        isGameOn = true;
        Time.timeScale = 1f;
    }

    void InitializeSettings() // 게임 매니저 내 모든 변수 값 false 또는 0으로 초기화.
    {
        isGameOn = false;
        isGameOver = false;

        score = 0;
        scoreText.GetComponent<Text>().text = "" + score;
        Color color_alphaZero = scoreText.GetComponent<Text>().color;
        color_alphaZero.a = 0.0f;
        scoreText.GetComponent<Text>().color = color_alphaZero;

        m_ball.GetComponent<BallScript>().SetBallSpeed(initBallSpeed);
        m_ball.GetComponent<BallScript>().SetBallVector(initBallVector);
        m_ball.GetComponent<BallScript>().SetBallPosition(initBallPosition);

        m_mainMenu.SetActive(false);
        m_GameOver.SetActive(false);
        m_GamePanel.SetActive(false);
    }
    public void HandleGameOver()
    {
        isGameOn = false;
        isGameOver = true;

        SetPanels(Panels.GameOver);

        if (score > highScore)
        {
            highScore = score;
        }

        finalScoreText.GetComponent<Text>().text = "" + score;
        highScoreText.GetComponent<Text>().text = "HIGH SCORE\n" + highScore;

        Time.timeScale = 0f;
    }

    void SetPanels(Panels panel){
        switch (panel)
        {
            case Panels.MainMenu:
                m_mainMenu.SetActive(true);
                m_GameOver.SetActive(false);
                m_GamePanel.SetActive(false);
                break;
            case Panels.GameOver:
                m_mainMenu.SetActive(false);
                m_GameOver.SetActive(true);
                m_GamePanel.SetActive(false);
                break;
            case Panels.GamePanel:
                m_mainMenu.SetActive(false);
                m_GameOver.SetActive(false);
                m_GamePanel.SetActive(true);
                break;
            default:
                break;
        }
    }
}
