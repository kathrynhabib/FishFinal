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

    //private fishDataController selectedFish;
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

        if (isPlayer)
        {
            SwitchManager.Instance.AssignPlayer(this);
            FishDiscoveryManager.Instance.Discover(FishData);
            SetPlayer();
        }
        else
        {
            SetAI();
        }
    }

    void Update()
    {
        if (!isPlayer) return;
        if (SwitchConfirmPopup.Instance != null && SwitchConfirmPopup.Instance.IsOpen) return;
        highlightNearby();
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
            {
                Destroy(child.gameObject);

                Debug.Log(gameObject.name + " was destroyed!");
            }

            GameObject model = Instantiate(FishData.modelPrefab, modelContainer);

            outline = model.GetComponent<Outline>();

            if (outline == null)
                outline = model.AddComponent<Outline>();

            outline.enabled = false;
        }

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

    public void setHighlight(Color color)
    {
        if (outline != null)
        {
            Debug.Log("highlighting this dih");
            outline.enabled = true;
            outline.OutlineColor = color;
        }
    }

    public void unhighlight()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    public void clearSelection(fishDataController fish)
    {
        if (highlightedFish.Contains(fish))
        {
            fish.setHighlight(Color.yellow);
        }
        else
        {
            fish.unhighlight();
        }
    }

    public void SetPlayer()
    {
        isPlayer = true;

        if (playerInput != null)
        {
            playerInput.enabled = true;
        }
        if (aiBehavior != null)
        {
            aiBehavior.disable();
        }

        // update tracker so other fish know who the player is
        PlayerFishTracker.Current = transform;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        SwitchManager.Instance.AssignPlayer(this);
    }

    public void SetAI()
    {
        isPlayer = false;

        if (playerInput != null)
        {
            playerInput.enabled = false;
        }
        if (aiBehavior != null)
        {
            aiBehavior.enable();
        }

        rb.interpolation = RigidbodyInterpolation.None;
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
    }
}

