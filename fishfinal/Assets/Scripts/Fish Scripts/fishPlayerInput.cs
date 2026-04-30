using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class fishPlayerInput : MonoBehaviour
{
    public fishMovement movement;
    public Transform fishCamera;

    private Vector3 inputDir;
    private Quaternion targetRotation;

    public static event Action OnSwitchRequested;

    //private FishAbilityController abilityController;

    void Awake()
    {
        movement = GetComponent<fishMovement>();
        //abilityController = GetComponent<FishAbilityController>();
    }

    void Update()
    {
        handleMovement();
        handleRotation();

        if (Input.GetKeyDown(KeyCode.E)) // might do e instead so lshift can be for dashing?
        {
            OnSwitchRequested?.Invoke();
        }

        /* if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            abilityController.ActivateAbility(0);
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt)) // transform
        {
            abilityController.ActivateAbility(1);
        }
        */
    }

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();

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
        targetRotation = Quaternion.LookRotation(fishCamera.forward, Vector3.up);
    }

    private void ApplyMovement()
    {
        movement.applyMovement(inputDir);
    }

    private void ApplyRotation()
    {
        movement.applyRotation(targetRotation); 
    }

}