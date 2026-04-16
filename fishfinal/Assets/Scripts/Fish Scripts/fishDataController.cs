using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


public class fishDataController : MonoBehaviour // contains the data bound to each fish and helps activate/ deactivate it as a player
{
    public FishData FishData;
    public bool isPlayer;
    public Camera mainCamera;

    //private fishBehavior aiBehavior; // ai behavior

    private fishBehavior aiBehavior;

    private fishPlayerInput playerInput;
    private fishMovement movement;

    // for adding confirmation for switching

    private fishDataController selectedFish;
    private Outline outline;

    // should also now provide camera offset dimensions to the cinemachine, along w a setting for mouse sens
    //private fishCameraController camControl;
    public GameObject thisPrefab;

    void Start()
    {
        
        
        playerInput = GetComponent<fishPlayerInput>();
        aiBehavior = GetComponent<fishBehavior>();
        movement = GetComponent<fishMovement>();
        outline = GetComponent<Outline>();
        //camControl = GetComponent<fishCameraController>();
        if (outline != null) outline.enabled = false;

        applyFishData();
        updateControlState();

        if (thisPrefab != null)
        {
            Debug.Log(FishData.fishName + "prefab imported");
        }

        if (isPlayer)
            FishDiscoveryManager.Instance.Discover(FishData);
    }

    void Update()
    {
        if (!isPlayer) return;

        if (SwitchConfirmPopup.Instance != null && SwitchConfirmPopup.Instance.IsOpen) return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            trySelectFish();
    }

    public void updateControlState()
    {
        if (playerInput != null)
            playerInput.enabled = isPlayer;

        if (aiBehavior != null)
        {
            if (!isPlayer) aiBehavior.enable();  
            else           aiBehavior.disable();
        }

        // update tracker so other fish know who the player is
        if (isPlayer)
            PlayerFishTracker.Current = transform;
    }

    void applyFishData()
    {
        if (FishData == null)
        {
            Debug.LogError("attach FishData pls");
            return;
        }

        movement.acceleration    = FishData.acceleration;
        movement.turnSpeed       = FishData.turnSpeed;
        movement.maxSpeed        = FishData.maxSpeed;
        movement.slowingSpeed    = FishData.slowingSpeed;
        movement.horizontalEnabled = FishData.horizontalEnabled;
        movement.verticalEnabled   = FishData.verticalEnabled;

        CapsuleCollider col = GetComponent<CapsuleCollider>();
        if (col != null)
            col.radius = FishData.colliderRadius;

        Transform modelContainer = transform.Find("ModelContainer");
        if (modelContainer != null)
        {
            foreach (Transform child in modelContainer)
                Destroy(child.gameObject);

            Instantiate(FishData.modelPrefab, modelContainer);
        }
    }

    // for switching, player presses shift to switch into a fish theyre looking at. if the other fish is roughly in the center of the screen (raycast), select.
    // hit left shift again to confirm switch
    private void trySelectFish()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.SphereCast(ray, 1.5f, out RaycastHit hit, 100f))
        {
            fishDataController targetFish = hit.collider.GetComponentInParent<fishDataController>();

            if (targetFish != null && targetFish != this && targetFish.tag == "fish")
            {
                selectedFish = targetFish;
                highlightSelection();


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

    void trySwitchFish(fishDataController target)
    {
        fishCameraController camControl = mainCamera.GetComponent<fishCameraController>();
        
        camControl.setTarget(target.thisPrefab.transform);
        camControl.setRadius(target.FishData.cameraRadius);
        // also set interp and collision detect

        isPlayer = false;
        updateControlState();       // AI on for old fish, playerInput off

        target.isPlayer = true;
        target.updateControlState(); // AI off for new fish, playerInput on

        FishDiscoveryManager.Instance.Discover(target.FishData);
    }

    void highlightSelection()
    {
        if (selectedFish != null && selectedFish.outline != null)
            selectedFish.outline.enabled = true;
    }

    void unhighlightSelection()
    {
        if (selectedFish != null && selectedFish.outline != null)
            selectedFish.outline.enabled = false;
    }

    void clearSelection()
    {
        unhighlightSelection();
        selectedFish = null;
    }
}

