using System.Runtime.CompilerServices;
using UnityEngine;

public class fishDataController : MonoBehaviour // contains the data bound to each fish and helps activate/ deactivate it as a player
{
    public fishData fishData;
    public bool isPlayer;
    public Camera mainCamera;
    public FishDiscoveryManager FishDiscoveryManager;

    private fishBehavior aiBehavior; // ai behavior
    private fishPlayerInput playerInput;

    // for adding confirmation for switching
    private fishDataController selectedFish;
    private Outline outline;

    void Start()
    {
        playerInput = GetComponent<fishPlayerInput>();
        aiBehavior = GetComponent<fishBehavior>();
        outline = GetComponent<Outline>();
        outline.enabled = false;
        updateControlState();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayer) return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            handleSelection();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            clearSelection();
        }
    }
    void updateControlState()
    {
        playerInput.enabled = isPlayer;

        if (!isPlayer) aiBehavior.enable();
        else aiBehavior.disable();
    }

    
    // for switching, player presses shift to switch into a fish theyre looking at. if the other fish is roughly in the center of the screen (raycast), select.
    // hit left shift again to confirm switch
    private void handleSelection()
    {
        if(selectedFish == null)
        {
            trySelectFish();
        }
        else
        {
            trySwitchFish(selectedFish);
            clearSelection();
        }
    }
    private void trySelectFish()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        float maxDistance = 100f;

        if (Physics.SphereCast(ray, 1.5f, out hit, maxDistance))
        {
            fishDataController targetFish = hit.collider.GetComponent<fishDataController>();

            if (targetFish != null && targetFish != this && targetFish.tag == "fish")
            {
                selectedFish = targetFish;
                Debug.Log("selected target fish, lshift again to confirm");
                // i think here we can also add a ui popup that temporarilty freezes the screen asking the player to confirm or exit
                highlightSelection();
            }
        }
    }

    void trySwitchFish(fishDataController selectedFish)
    {
        // enable fishMotion when switching to ai behavior
        // not using fishMotion for player behavior
        fishCameraController camera = mainCamera.GetComponent<fishCameraController>();
        camera.target = selectedFish.transform;

        isPlayer = false;
        updateControlState();

        selectedFish.isPlayer = true;
        selectedFish.updateControlState();
        FishDiscoveryManager.Instance.Discover(selectedFish.fishData); 

    }

    void highlightSelection()
    {
        Debug.Log("highlighting selected fish");
        if (outline != null)
        {
            selectedFish.outline.enabled = true;
        }
    }

    void unhighlightSelection()
    {
        Debug.Log("unhighlighting");
        if (outline != null)
        {
            selectedFish.outline.enabled = false;
        }
    }

    void clearSelection()
    {
        unhighlightSelection();
        selectedFish = null;
    }


}
