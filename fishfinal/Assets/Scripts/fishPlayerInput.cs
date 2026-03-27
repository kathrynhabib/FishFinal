using UnityEngine;
using System.Collections;

public class fishPlayerInput : MonoBehaviour
{

    public Transform fishCamera;
    public float acceleration = 10;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveLeftRight = Input.GetAxis("Horizontal");
        float moveForwardBack = Input.GetAxis("Vertical");

        Vector3 xAcceleration = fishCamera.right * moveLeftRight * Time.deltaTime * acceleration;
        Vector3 forward = fishCamera.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 zAcceleration = forward * moveForwardBack * Time.deltaTime * acceleration;

        rb.angularVelocity += xAcceleration + zAcceleration;
    }
}