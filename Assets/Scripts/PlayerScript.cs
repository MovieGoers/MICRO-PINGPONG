using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private static PlayerScript instance;

    GameObject ball;

    public GameObject whitePlane;
    public Vector3 originalPlayerScale;

    public int addingScore;

    public int tempScore;
    public static PlayerScript Instance
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

        ball = BallScript.Instance.gameObject;
    }
    private void Start()
    {
        addingScore = 1;
        tempScore = 0;
        originalPlayerScale = transform.localScale;
    }
    private void Update()
    {
        if (GameManager.Instance.DebugMode)
        {
            transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, transform.position.z);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball")) // 플레이어가 공에 닿을 경우,
        {
            GameManager.Instance.score += addingScore;
            tempScore += addingScore;
            BallScript.Instance.AddBallSpeed(GameManager.Instance.ballSpeedIncrement);
            BallScript.Instance.RotateBallVector(Random.Range(0f, 360f));

            StartCoroutine(UIManager.Instance.ShowScore());

            AudioManager.Instance.Play("Score");
            StartCoroutine(CameraManager.Instance.ShakeCamera(0.2f, 0.2f));

            StartCoroutine(PlayHitEffect(0.09f));

            
            if(tempScore >= 10) // 10점 낼때마다 효과.
            {
                tempScore -= 10;
                StartCoroutine(GameManager.Instance.SlowTime(0.1f, 1.5f));
                AudioManager.Instance.Play("Explosion");
                AudioManager.Instance.Play("Milestone");

                ParticleManager.Instance.SetItemSpark(ball.transform.position);
                ParticleManager.Instance.PlayItemSpark();
            }
        }
    }

    IEnumerator PlayHitEffect(float planeScale)
    {
        yield return null;
        whitePlane.transform.localScale = new Vector3(planeScale, 1, planeScale);
        while (whitePlane.transform.localScale.x > 0)
        {
            yield return new WaitForSeconds(0.01f);
            whitePlane.transform.localScale -= new Vector3(0.005f, 0, 0.005f);
        }
        whitePlane.transform.localScale = new Vector3(0, 1, 0);
    }
}
