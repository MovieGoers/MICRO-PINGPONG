using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public float ballSpeed;

    public Vector3 ballMovementVector;

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
        if (go.CompareTag("Wall") || go.CompareTag("Player"))
        {
            if (go.transform.up.x < -0.01f || go.transform.up.x > 0.01f)
                ballMovementVector.x *= -1;
            else if (go.transform.up.y < -0.01f || go.transform.up.y > 0.01f)
                ballMovementVector.y *= -1;
            else if (go.transform.up.z < -0.01f || go.transform.up.z > 0.01f)
                ballMovementVector.z *= -1;
        }
    }
}
