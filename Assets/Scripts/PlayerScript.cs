using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    GameObject ball;
    private void Awake()
    {
        ball = GameObject.Find("Ball");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball")) // 플레이어가 공에 닿을 경우,
        {
            GameManager.Instance.score += 1;

            ball.GetComponent<BallScript>().AddBallSpeed(GameManager.Instance.ballSpeedIncrement);
            ball.GetComponent<BallScript>().RotateBallVector(Random.Range(0f, 360f));
            StartCoroutine(UIManager.Instance.ShowScore());
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }
}
