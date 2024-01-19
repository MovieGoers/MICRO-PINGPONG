using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    GameObject m_ball;
    GameObject m_player;
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
        GameOverState
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
        m_player = GameObject.Find("Player");
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
    }
    
    public void StartGame()
    {
        gameState = GameStates.GameState;

        UIManager.Instance.HideCursor();
        UIManager.Instance.SetPanels(UIManager.Panels.GamePanel);

        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        InitializeSettings();

        gameState = GameStates.GameState;

        UIManager.Instance.HideCursor();
        UIManager.Instance.SetPanels(UIManager.Panels.GamePanel);

        Time.timeScale = 1f;
    }

    void InitializeSettings() // ���� �Ŵ��� �� ���� �� false �Ǵ� 0���� �ʱ�ȭ.
    {
        score = 0;

        UIManager.Instance.SetScoreText(score);
        UIManager.Instance.SetScoreTextAlpha(0f);
        UIManager.Instance.SetPanels(UIManager.Panels.SetAllPanelsFalse);

        m_ball.GetComponent<BallScript>().SetBallSpeed(initBallSpeed);

        m_ball.GetComponent<BallScript>().SetBallVector(initBallVector);
        Quaternion randomQuatZAxis = Quaternion.Euler(0, 0, 90 * Random.Range(0, 5)); // z �� ���� ���� ȸ�� ���ʹϾ� ����.
        m_ball.GetComponent<BallScript>().ballMovementVector = randomQuatZAxis * m_ball.GetComponent<BallScript>().ballMovementVector; // ���� ȸ��

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
    }
}
