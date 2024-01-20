using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    float ballSpeed;
    public Vector3 ballMovementVector;
    public GameObject ballLine;
    Vector3 ballPosition;

    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ballLine = GameObject.Find("Ball Line");
    }

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(ballMovementVector.normalized * ballSpeed * Time.deltaTime);
        ballLine.transform.position = new Vector3(ballLine.transform.position.x, ballLine.transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Wall") || go.CompareTag("Player")) // �ε��� �鿡 ���� ���� ��ȯ
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
        
        if(go.CompareTag("Invisible Collider")) //  ���� ����
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

    public void RotateBallVector(float angle)
    {
        Quaternion randomQuatZAxis = Quaternion.Euler(0, 0, angle); // z �� ���� ���� ȸ�� ���ʹϾ� ����.
        ballMovementVector = randomQuatZAxis * ballMovementVector; // ���� ȸ��
    }
}
