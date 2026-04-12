using System.Runtime.CompilerServices;
using UnityEngine;

public class fishDataController : MonoBehaviour // contains the data bound to each fish and helps activate/ deactivate it as a player
{
    public FishData FishData;
    public bool isPlayer;
    public Camera mainCamera;

    private fishBehavior aiBehavior; // ai behavior
    private fishPlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<fishPlayerInput>();
        aiBehavior = GetComponent<fishBehavior>();
        updateControlState();

        Debug.Log("fishDataController Start on: " + gameObject.name + " | isPlayer: " + isPlayer);
        Debug.Log("FishDiscoveryManager.Instance is: " + FishDiscoveryManager.Instance); //will delete

        if (isPlayer)
            FishDiscoveryManager.Instance.Discover(FishData);
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

        if (!isPlayer) aiBehavior.enable();
        else aiBehavior.disable();
    }

    // for switching, player presses shift to switch into a fish theyre looking at. if the other fish is roughly in the center of the screen (raycast), allow switch
    private void trySwitchFish()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        float maxDistance = 100f;

        if (Physics.SphereCast(ray, 1.5f, out hit, maxDistance))
        {

            fishDataController targetFish = hit.collider.GetComponent<fishDataController>();

            if (targetFish != null && targetFish != this && targetFish.tag == "fish")
            {
                playerSwitch(targetFish);
            }
        }
    }

    void playerSwitch(fishDataController target)
    {
        // enable fishMotion when switching to ai behavior
        // not using fishMotion for player behavior
        Debug.Log("playerSwitch called, switching to: " + target.gameObject.name); //will delete
        fishCameraController camera = mainCamera.GetComponent<fishCameraController>();
        camera.target = target.transform;

        isPlayer = false;
        updateControlState();

        target.isPlayer = true;
        target.updateControlState();
        FishDiscoveryManager.Instance.Discover(target.FishData); 

    }


}
