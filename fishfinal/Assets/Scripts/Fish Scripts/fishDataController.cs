using NUnit.Framework;
using System.Collections.Generic;
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
    public GameObject thisPrefab;
    private Rigidbody rb;

    // for adding highlights to selectable fish
    public float selectableRadius;
    private List<fishDataController> highlightedFish = new List<fishDataController>();
    public LayerMask fishLayer;

    void Start()
    {
        
        playerInput = GetComponent<fishPlayerInput>();
        aiBehavior = GetComponent<fishBehavior>();
        movement = GetComponent<fishMovement>();
        outline = GetComponent<Outline>();
        rb = GetComponent<Rigidbody>();

        if (outline != null)
        {
            outline.enabled = false;
        }

        applyFishData();
        updateControlState();


        if (isPlayer)
            FishDiscoveryManager.Instance.Discover(FishData);
    }

    void Update()
    {
        if (!isPlayer) return;

        if (SwitchConfirmPopup.Instance != null && SwitchConfirmPopup.Instance.IsOpen) return;

        highlightNearby();
        
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
            Debug.LogError("attach" + this.name + " FishData pls");
            return;
        }

        movement.acceleration    = FishData.acceleration;
        movement.turnSpeed       = FishData.turnSpeed;
        movement.maxSpeed        = FishData.maxSpeed;
        movement.drag              = FishData.slowingSpeed;
        movement.horizontalEnabled = FishData.horizontalEnabled;
        movement.verticalEnabled   = FishData.verticalEnabled;

        CapsuleCollider col = GetComponent<CapsuleCollider>();
        if (col != null)
            col.radius = FishData.colliderRadius;

        /* the 13 lines below are AI generated*/
        Transform modelContainer = transform.Find("ModelContainer");
        if (modelContainer != null)
        {
            foreach (Transform child in modelContainer)
                Destroy(child.gameObject);

            GameObject model = Instantiate(FishData.modelPrefab, modelContainer);

            outline = model.GetComponent<Outline>();

            if (outline == null)
                outline = model.AddComponent<Outline>();

            outline.enabled = false;
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
                selectedFish.setHighlight(Color.white);
                Debug.Log("making selected fish white");

                SwitchConfirmPopup.Instance.Show(targetFish.FishData.fishName,
                    () =>
                    {
                        Debug.LogError("first selection screen triggered"); 
                        trySwitchFish(targetFish);
                        clearSelection();
                    },
                    () =>
                    {
                        Debug.LogError("cancel screen triggered"); 
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
        rb.interpolation = RigidbodyInterpolation.None;
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

        target.rb.interpolation = RigidbodyInterpolation.Interpolate;
        target.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;


        isPlayer = false;
        updateControlState();       // AI on for old fish, playerInput off

        target.isPlayer = true;
        target.unhighlight();
        target.updateControlState(); // AI off for new fish, playerInput on

        FishDiscoveryManager.Instance.Discover(target.FishData);
    }

    void highlightNearby()
    {
        foreach (var fish in highlightedFish) // refreshing
        {
            if (fish != null)
            {
                fish.unhighlight();
            }
        }
        highlightedFish.Clear();

        Collider[] hits = Physics.OverlapSphere(transform.position, selectableRadius, fishLayer);

        foreach (var hit in hits)
        {
            fishDataController toHighlight = hit.GetComponent<fishDataController>();
            if (toHighlight != null && toHighlight != this)
            {
                toHighlight.setHighlight(Color.yellow);
                highlightedFish.Add(toHighlight);

                Debug.Log(toHighlight.FishData.fishName);
            }
        }

    }

    void setHighlight(Color color)
    {
        if (outline != null)
        {
            Debug.Log("highlighting this dih");
            outline.enabled = true;
            outline.OutlineColor = color;
        }
    }

    void unhighlight()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    void clearSelection()
    {
        if (highlightedFish.Contains(selectedFish))
        {
            selectedFish.setHighlight(Color.yellow);
        }
        else
        {
            selectedFish.unhighlight();
        }
        selectedFish = null;
    }
}

