using System.Runtime.CompilerServices;
using UnityEngine;

public class fishDataController : MonoBehaviour // contains the data bound to each fish and helps activate/ deactivate it as a player
{
    public fishData fishData;
    public bool isPlayer;
    public Camera mainCamera;
    public FishDiscoveryManager FishDiscoveryManager;

    private fishMovement movement;
    private fishBehavior behavior; // ai behavior
    private fishPlayerInput playerInput;

    void Start()
    {
        movement = GetComponent<fishMovement>();
        playerInput = GetComponent<fishPlayerInput>();
        behavior = GetComponent<fishBehavior>();
        updateControlState();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayer) return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            trySwitchFish();
        }
    }
    void updateControlState()
    {
        playerInput.enabled = isPlayer;
        behavior.enabled = !isPlayer;
    }

    // for switching, player presses shift to switch into a fish theyre looking at. if the other fish is roughly in the center of the screen (raycast), allow switch
    private void trySwitchFish()
    {
        Debug.Log("checking if switchable");
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        float maxDistance = 100f;

        if (Physics.SphereCast(ray, 1.5f, out hit, maxDistance))
        {
            Debug.Log("spherecast hit!");

            fishDataController targetFish = hit.collider.GetComponent<fishDataController>();

            if (targetFish != null && targetFish != this && targetFish.tag == "fish")
            {
                Debug.Log("switching to target fish");
                playerSwitch(targetFish);
            }
        }
    }

    void playerSwitch(fishDataController target)
    {
        fishCameraController camera = mainCamera.GetComponent<fishCameraController>();
        camera.target = target.transform;

        isPlayer = false;
        updateControlState();

        target.isPlayer = true;
        target.updateControlState();
        FishDiscoveryManager.Instance.Discover(target.fishData); 

    }


}
