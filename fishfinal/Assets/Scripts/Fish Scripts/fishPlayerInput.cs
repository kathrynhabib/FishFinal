using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class fishPlayerInput : MonoBehaviour
{
    public fishMovement movement;
    public Transform fishCamera;

    void Awake()
    {
        movement = GetComponent<fishMovement>();
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
        
        Debug.Log("input H: " + moveLeftRight + " V: " + moveForwardBack + " camera forward: " + fishCamera.forward);

        Vector3 inputDir = fishCamera.right * moveLeftRight + fishCamera.forward * moveForwardBack + fishCamera.up * moveUpDown;

        movement.applyMovement(inputDir);

    }
    void handleRotation() 
    {
        Quaternion targetRotation = Quaternion.LookRotation(fishCamera.forward, Vector3.up);
        movement.applyRotation(targetRotation);
    }

}