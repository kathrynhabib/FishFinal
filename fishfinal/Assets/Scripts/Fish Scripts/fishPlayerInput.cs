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

        Vector3 move = fishCamera.right * moveLeftRight + fishCamera.forward * moveForwardBack;
        movement.applyMovement(move);
    }
}