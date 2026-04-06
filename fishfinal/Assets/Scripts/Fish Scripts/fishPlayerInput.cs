using UnityEngine;
using System.Collections;

public class fishPlayerInput : MonoBehaviour
{

    public Transform fishCamera;

    public float acceleration = 10;
    public float turnSpeed = 3f; // turning speed of the fish
    public float slowingSpeed = .99f;
    public float maxSpeed = 1.5f; // should vary per fish

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        handleMovement();
        handleRotation();

    }
    void handleMovement()
    {
        float moveLeftRight = Input.GetAxis("Horizontal");
        float moveForwardBack = Input.GetAxis("Vertical");

        float moveUpDown = 0f;

        if (Input.GetKey(KeyCode.Space))
            moveUpDown += 1f;

        if (Input.GetKey(KeyCode.LeftControl))
            moveUpDown -= 1f;


        Vector3 move = fishCamera.right * moveLeftRight + fishCamera.forward * moveForwardBack + fishCamera.up * moveUpDown;
        if (move.magnitude > 0.01f && move.magnitude < maxSpeed)
        {
            Vector3 accel = move * Time.deltaTime * acceleration;
            rb.linearVelocity += accel;
        }
        else
        {
            rb.linearVelocity *= slowingSpeed;
        }
    }
    void handleRotation() 
    {

        Quaternion targetRotation = Quaternion.LookRotation(fishCamera.forward, Vector3.up);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.deltaTime);

    }

}