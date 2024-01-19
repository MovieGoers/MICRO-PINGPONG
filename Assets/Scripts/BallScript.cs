using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    float ballSpeed;
    public Vector3 ballMovementVector;
    Vector3 ballPosition;

    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(ballMovementVector.normalized * ballSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Wall") || go.CompareTag("Player")) // 부딪힌 면에 대한 방향 변환
        {
            if (go.transform.up.x < -0.01f || go.transform.up.x > 0.01f)
                ballMovementVector.x *= -1;
            else if (go.transform.up.y < -0.01f || go.transform.up.y > 0.01f)
                ballMovementVector.y *= -1;
            else if (go.transform.up.z < -0.01f || go.transform.up.z > 0.01f)
                ballMovementVector.z *= -1;
        }
        
        if(go.CompareTag("Invisible Collider")) //  게임 오버
        {
            GameManager.Instance.HandleGameOver();
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
}
