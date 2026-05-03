using UnityEngine;
using System.Collections;
using FishAlive;

public class fishPlayerInput_2 : MonoBehaviour
{

    public Transform fishCamera;
    public FishMotion fishMotion;

    public float acceleration = 10;
    public float turnSpeed = 3f; // turning speed of the fish
    public float slowingSpeed = .99f;
    public float maxSpeed = 1.5f; // should vary per fish

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        fishMotion = GetComponent<FishMotion>();
        fishMotion.SetAutoMotion(false);
        rb.isKinematic = true;
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
        if (move.magnitude > 0.01f)
        {
            fishMotion.StartTurnTowardsDirection(move.normalized);
            fishMotion.SetMotionForce(acceleration);
        }
        else
        {
            fishMotion.SetMotionForce(0);
        }
    }
    void handleRotation()
    {

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            Vector3 forward = fishCamera.forward;
            fishMotion.StartTurnTowardsDirection(forward, true);
        }

    }

}