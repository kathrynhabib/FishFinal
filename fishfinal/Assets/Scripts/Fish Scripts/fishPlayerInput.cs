using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class fishPlayerInput : MonoBehaviour
{
    public fishMovement movement;
    public Transform fishCamera;

    private Vector3 inputDir;

    void Awake()
    {
        movement = GetComponent<fishMovement>();
    }

    void Update()
    {
        handleMovement();
        handleRotation();
    }

    void FixedUpdate()
    {
        ApplyMovement();
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


        inputDir = fishCamera.right * moveLeftRight + fishCamera.forward * moveForwardBack + fishCamera.up * moveUpDown;
    }
    void handleRotation() 
    {
        Quaternion targetRotation = Quaternion.LookRotation(fishCamera.forward, Vector3.up);
        movement.applyRotation(targetRotation);
    }

    private void ApplyMovement()
    {
        movement.applyMovement(inputDir);
    }

}