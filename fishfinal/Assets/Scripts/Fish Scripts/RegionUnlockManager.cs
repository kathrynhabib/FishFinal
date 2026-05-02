using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

[System.Serializable]
public class RegionUnlockData
{
    public string regionName;
    public CinemachineCamera unlockCamera;
    public Animator wallAnimator;
    public string openTrigger = "Open";
    public float animationLength = 3.0f;
    public Collider wallCollider;
}

public class RegionUnlockManager : MonoBehaviour
{
    public static RegionUnlockManager Instance { get; private set; }

    [Header("Player Camera")]
    public CinemachineCamera playerCamera;

    [Header("Regions")]
    public List<RegionUnlockData> regions;

    private fishPlayerInput playerInput;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        playerInput = FindObjectOfType<fishPlayerInput>();

        foreach (var region in regions)
            SetCameraDormant(region);

        // wire up each region's completion event to its unlock sequence
        var discoveryRegions = FishDiscoveryManager.Instance.regions;
        for (int i = 0; i < discoveryRegions.Count; i++)
        {
            int index = i; // capture for lambda
            discoveryRegions[i].onCompleted.AddListener(
                () => StartCoroutine(UnlockSequence(regions[index]))
            );
        }
    }

    // delete this when done testing
    void Update()
    {
        for (int i = 0; i < regions.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                StartCoroutine(UnlockSequence(regions[i]));
        }
    }

    void SetCameraDormant(RegionUnlockData region)
    {
        if (region.unlockCamera != null)
            region.unlockCamera.Priority = -1;
    }

    IEnumerator UnlockSequence(RegionUnlockData region)
    {
        // 1. Disable player input
        if (playerInput != null) playerInput.enabled = false;

        // 2. Switch to unlock camera
        region.unlockCamera.Priority = 99;

        // 3. Wait for blend to finish
        yield return new WaitUntil(() => region.unlockCamera.Priority > playerCamera.Priority);
        yield return new WaitForSeconds(1.2f);

        // 4. Trigger wall animation
        if (region.wallAnimator != null)
            region.wallAnimator.SetTrigger(region.openTrigger);

        // 5. Wait for animation
        yield return new WaitForSeconds(region.animationLength);

        // 6. Disable wall collider
        if (region.wallCollider != null)
            region.wallCollider.enabled = false;

        // 7. Return camera to player
        region.unlockCamera.Priority = -1;

        // 8. Wait for blend back
        yield return new WaitForSeconds(1f);

        // 9. Re-enable player
        if (playerInput != null) playerInput.enabled = true;
    }
}