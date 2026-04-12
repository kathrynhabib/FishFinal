using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class fishDataController : MonoBehaviour // contains the data bound to each fish and helps activate/ deactivate it as a player
{
    public FishData FishData;
    public bool isPlayer;
    public Camera mainCamera;

    //private fishBehavior aiBehavior; // ai behavior
    private fishPlayerInput playerInput;
    private fishMovement movement;

    // for adding confirmation for switching
    private fishDataController selectedFish;
    private Outline outline;

    void Start()
    {
        playerInput = GetComponent<fishPlayerInput>(); // restructure these to take their data from fishdata too?
        //aiBehavior = GetComponent<fishBehavior>();
        movement = GetComponent<fishMovement>();
        outline = GetComponent<Outline>();
        outline.enabled = false;

        applyFishData();
        updateControlState();

        Debug.Log("fishDataController Start on: " + gameObject.name + " | isPlayer: " + isPlayer);
        Debug.Log("FishDiscoveryManager.Instance is: " + FishDiscoveryManager.Instance); //will delete

        //if (isPlayer)
        //    FishDiscoveryManager.Instance.Discover(FishData);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayer) return;

        if (SwitchConfirmPopup.Instance != null && SwitchConfirmPopup.Instance.IsOpen) return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            trySelectFish();
        }

    }
    void updateControlState()
    {
        playerInput.enabled = isPlayer;

        //if (!isPlayer) aiBehavior.enable();
        //else aiBehavior.disable();
    }

    void applyFishData()
    {
        if (FishData == null)
        {
            Debug.LogError("attach FishData pls");
            return;
        }
        // these should all be replaced w adjustments in fishMovement isntead
        movement.acceleration = FishData.acceleration;
        movement.turnSpeed = FishData.turnSpeed;
        movement.maxSpeed = FishData.maxSpeed;
        movement.slowingSpeed = FishData.slowingSpeed;
        movement.horizontalEnabled = FishData.horizontalEnabled;
        movement.verticalEnabled = FishData.verticalEnabled;

        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        if (collider != null)
        {
            collider.radius = FishData.colliderRadius;
        }

        Transform modelContainer = transform.Find("ModelContainer");
        if(modelContainer != null)
        {
            foreach (Transform child in modelContainer) // prevents duplicates when switching fish
                Destroy(child.gameObject);
            
            Instantiate(FishData.modelPrefab, modelContainer);
        }

        //aiBehavior = GetComponent<fishBehavior>();
        //aiBehavior.SetBehavior(FishData.behaviorType);
    }

    // for switching, player presses shift to switch into a fish theyre looking at. if the other fish is roughly in the center of the screen (raycast), select.
    // hit left shift again to confirm switch
    private void trySelectFish()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        float maxDistance = 100f;

        if (Physics.SphereCast(ray, 1.5f, out hit, maxDistance))
        {

            fishDataController targetFish = hit.collider.GetComponentInParent<fishDataController>();

            if (targetFish != null && targetFish != this && targetFish.tag == "fish")
            {
                selectedFish = targetFish;
                highlightSelection();

                Debug.Log("selected target fish, lshift again to confirm");
                // i think here we can also add a ui popup that temporarilty freezes the screen asking the player to confirm or exit
                SwitchConfirmPopup.Instance.Show(targetFish.FishData.fishName,
                    () =>
                    {
                        trySwitchFish(targetFish);
                        clearSelection();
                    },
                    () =>
                    {
                        clearSelection();
                    }
                );

            }
        }
    }

    void trySwitchFish(fishDataController selectedFish)
    {
        // enable fishMotion when switching to ai behavior
        // not using fishMotion for player behavior
        Debug.Log("playerSwitch called, switching to: " + selectedFish.gameObject.name); //will delete
        fishCameraController camera = mainCamera.GetComponent<fishCameraController>();
        camera.target = selectedFish.transform;

        isPlayer = false;
        updateControlState();

        selectedFish.isPlayer = true;
        selectedFish.updateControlState();
        FishDiscoveryManager.Instance.Discover(selectedFish.FishData);

    }

    void highlightSelection()
    {
        Debug.Log("highlighting selected fish");
        if (selectedFish.outline != null)
        {
            selectedFish.outline.enabled = true;
        }
    }

    void unhighlightSelection()
    {
        Debug.Log("unhighlighting");
        if (selectedFish.outline != null)
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