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
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameManager.Instance.score += 1;

            ball.GetComponent<BallScript>().AddBallSpeed(GameManager.Instance.ballSpeedIncrement);
            StartCoroutine(ShowScore());
        }
    }

    IEnumerator ShowScore()
    {
        yield return null;
        GameManager.Instance.scoreText.GetComponent<Text>().text = "" + GameManager.Instance.score;

        float text_alpha = 1.0f;
        while(text_alpha > 0.0f)
        {
            Color new_color = GameManager.Instance.scoreText.GetComponent<Text>().color;
            new_color.a = text_alpha;
            GameManager.Instance.scoreText.GetComponent<Text>().color = new_color;

            text_alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
