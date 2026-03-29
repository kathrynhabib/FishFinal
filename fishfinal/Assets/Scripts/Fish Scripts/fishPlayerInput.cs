using UnityEngine;
using System.Collections;

public class fishPlayerInput : MonoBehaviour
{

    public Transform fishCamera;
    private fishMovement movement;

    void Awake()
    {
        movement = GetComponent<fishMovement>();
    }

    void Update()
    {
        float moveLeftRight = Input.GetAxis("Horizontal");
        float moveForwardBack = Input.GetAxis("Vertical");

        float moveUpDown = 0f;

        if (Input.GetKey(KeyCode.Space))
            moveUpDown += 1f;

        if (Input.GetKey(KeyCode.LeftControl))
            moveUpDown -= 1f;


        Vector3 move = fishCamera.right * moveLeftRight + fishCamera.forward * moveForwardBack + fishCamera.up * moveUpDown;
        movement.applyMovement(move);
    }
}