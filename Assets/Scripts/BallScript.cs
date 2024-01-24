using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    float ballSpeed;
    public Vector3 ballMovementVector;
    public GameObject ballLine;

    public Vector3 originalBallScale;

    Vector3 m_ballPosition;

    Rigidbody rb;

    private static BallScript instance;

    public static BallScript Instance
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
        rb = GetComponent<Rigidbody>();
        ballLine = GameObject.Find("Ball Line");
    }

    void Start()
    {
        originalBallScale = transform.localScale;
    }

    void Update()
    {
        transform.Translate(ballMovementVector.normalized * ballSpeed * Time.deltaTime);

        ballLine.transform.position = new Vector3(ballLine.transform.position.x, ballLine.transform.position.y, transform.position.z);

        // 공이 게임 맵 밖으로 나가는 버그 처리.

        if (transform.position.x < GameManager.Instance.wallLeft.transform.position.x
            || transform.position.x > GameManager.Instance.wallRight.transform.position.x)
        {
            transform.Translate(-1 * ballMovementVector.normalized * ballSpeed * Time.deltaTime);
            ballMovementVector.x *= -1;
        }

        if (transform.position.y < GameManager.Instance.wallBottom.transform.position.y
            || transform.position.y > GameManager.Instance.wallTop.transform.position.y)
        {
            transform.Translate(-1 * ballMovementVector.normalized * ballSpeed * Time.deltaTime);
            ballMovementVector.y *= -1;
        }

        if(transform.position.z > GameManager.Instance.wallBack.transform.position.z)
        {
            transform.Translate(-1 * ballMovementVector.normalized * ballSpeed * Time.deltaTime);
            ballMovementVector.z *= -1;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Wall")) // 부딪힌 면에 대한 방향 변환
        {
            if (go.transform.up.x < -0.01f || go.transform.up.x > 0.01f)
                ballMovementVector.x *= -1;
            else if (go.transform.up.y < -0.01f || go.transform.up.y > 0.01f)
                ballMovementVector.y *= -1;
            else if (go.transform.up.z < -0.01f || go.transform.up.z > 0.01f)
                ballMovementVector.z *= -1;

            AudioManager.Instance.Play("Bump");

            if (go.CompareTag("Wall"))
            {
                StartCoroutine(CameraManager.Instance.ShakeCamera(0.1f, 0.03f));
                ParticleManager.Instance.SetSparkPosition(transform.position);
                ParticleManager.Instance.PlaySpark();
            }
        }

         if( go.CompareTag("Player")){
            AudioManager.Instance.Play("Bump");
            if(ballMovementVector.z < 0)
                ballMovementVector.z *= -1;
         }

        if (go.CompareTag("Invisible Collider")) //  게임 오버
        {
            GameManager.Instance.HandleGameOver();
        }

        if (go.CompareTag("Edge Collider"))
        {
            Debug.Log("Entered Edge Collider!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;
        if (go.CompareTag("Item"))
        {
            // 아이템 효과, 파티클 실행.
            ItemManager.Instance.ActivateItemEffect(go.name);
            go.SetActive(false);
        }

    }

    public void AddBallSpeed(float value)
    {
        ballSpeed += value;
    }

    public void SetBallPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void SetBallSpeed(float speed)
    {
        ballSpeed = speed;
    }

    public void SetBallVector(Vector3 vec)
    {
        ballMovementVector = vec;
    }

    public void RotateBallVector(float angle)
    {
        Quaternion randomQuatZAxis = Quaternion.Euler(0, 0, angle); // z 축 기준 랜덤 회전 쿼터니언 생성.
        ballMovementVector = randomQuatZAxis * ballMovementVector; // 벡터 회전
    }

    public void SetBallSize(Vector3 scale)
    {
        transform.transform.localScale = scale;
    }

    public GameObject GetBallObject()
    {
        return gameObject;
    }
}
