using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    GameObject m_ball;
    
    public GameObject scoreText;
    public GameObject cube;

    public float initBallSpeed;
    public float ballSpeedIncrement;

    public Vector3 m_initBallVector;

    public int score;
    
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
        scoreText = GameObject.Find("Canvas/Score");
    }
    void Start()
    {
        score = 0;
        scoreText.GetComponent<Text>().text = "" + score;
        Color color_alphaZero = scoreText.GetComponent<Text>().color;
        color_alphaZero.a = 0.0f;
        scoreText.GetComponent<Text>().color = color_alphaZero;

        m_ball.GetComponent<BallScript>().ballSpeed = initBallSpeed;
        m_ball.GetComponent<BallScript>().ballMovementVector = m_initBallVector;
    }

    void Update()
    {
    }
}
