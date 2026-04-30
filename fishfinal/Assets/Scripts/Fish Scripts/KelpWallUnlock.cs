using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class KelpWallUnlock : MonoBehaviour
{
    [Header("Cinemachine")]
    public CinemachineCamera kelpWallCamera;   // drag KelpWallCamera here
    public CinemachineCamera playerCamera;     // drag your existing CinemachineCamera here

    [Header("Kelp Wall")]
    public Animator kelpWallAnimator;
    public string openAnimationTrigger = "Open";
    public float openAnimationLength = 3.0f;
    public Collider kelpWallCollider;

    void Start()
    {
        kelpWallCamera.Priority = -1;
        FishDiscoveryManager.Instance.onCoralReefCompleted.AddListener(OnCoralReefCompleted);
    }

    void OnCoralReefCompleted()
    {
        StartCoroutine(UnlockSequence());
    }

    IEnumerator UnlockSequence()
    {
        // 1. Disable player input
        fishPlayerInput playerInput = FindObjectOfType<fishPlayerInput>();
        if (playerInput != null) playerInput.enabled = false;

        // 2. Switch to kelp wall camera
        kelpWallCamera.Priority = 99;

        // 3. Wait until kelp wall camera is the highest priority (one frame for blend to start)
        yield return null;
        yield return new WaitUntil(() => kelpWallCamera.Priority > playerCamera.Priority);

        // small pause to let the shot settle
        yield return new WaitForSeconds(0.8f);

        // 4. Trigger animation
        kelpWallAnimator.SetTrigger(openAnimationTrigger);

        // 5. Wait for animation to finish
        yield return new WaitForSeconds(openAnimationLength);

        // 6. Remove blocker collider
        if (kelpWallCollider != null)
            kelpWallCollider.enabled = false;

        // 7. Hand camera back
        kelpWallCamera.Priority = -1;

        // 8. Brief wait for blend back
        yield return new WaitForSeconds(1f);

        // 9. Re-enable player
        if (playerInput != null) playerInput.enabled = true;
    }
}