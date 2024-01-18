using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    GameObject m_ball;

    public GameObject cube;

    public float initBallSpeed;
    public Vector3 m_initBallvelocity;
    
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
        m_ball.GetComponent<BallScript>().ballSpeed = initBallSpeed;
        m_ball.GetComponent<BallScript>().ballVelocity = m_initBallvelocity;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
