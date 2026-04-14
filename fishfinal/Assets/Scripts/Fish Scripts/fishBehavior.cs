using FishAlive;
using UnityEngine;
using UnityEngine.InputSystem;

public class fishBehavior : MonoBehaviour
{
    [Header("Detection")]
    public float detectionRadius = 6f;
    public float safeRadius = 10f;

    [Header("Idle")]
    public float idleMinDuration = 1f;
    public float idleMaxDuration = 3f;

    private fishMovement movement;
    private fishPatrol patrol;
    private Transform player;
    private float stateTimer;
    private FishState currentState = FishState.Idle;

    void Awake()
    {
        movement = GetComponent<fishMovement>();
        patrol = GetComponent<fishPatrol>();
    }

    public void enable()
    {
        enabled = true;
        EnterState(FishState.Patrol);
        Debug.Log("AI enabled on: " + gameObject.name);
    }

    public void disable()
    {
        enabled = false;
        movement.applyMovement(Vector3.zero);
        Debug.Log("AI disabled on: " + gameObject.name);
    }

    void Update()
    {
        if (PlayerFishTracker.Current != null)
            player = PlayerFishTracker.Current;

        if (player != null && player == transform)
            return;

        float dist = player != null
            ? Vector3.Distance(transform.position, player.position)
            : float.MaxValue;

        switch (currentState)
        {
            case FishState.Idle:   UpdateIdle(dist);   break;
            case FishState.Patrol: UpdatePatrol(dist); break;
            case FishState.Flee:   UpdateFlee(dist);   break;
        }
    }

    void UpdateIdle(float dist)
    {
        movement.applyMovement(Vector3.zero);
        stateTimer -= Time.deltaTime;

        if (dist < detectionRadius) { EnterState(FishState.Flee);   return; }
        if (stateTimer <= 0f)       { EnterState(FishState.Patrol); return; }
    }

    void UpdatePatrol(float dist)
    {
        if (dist < detectionRadius) { EnterState(FishState.Flee); return; }

        Vector3 target = patrol.GetPatrolTarget(transform.position);
        Vector3 dir = (target - transform.position).normalized;
        dir = SteerAroundObstacles(dir);
        movement.applyMovement(dir);

        if (dir.magnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            movement.applyRotation(targetRot);
        }

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f) { EnterState(FishState.Idle); return; }
    }

    void UpdateFlee(float dist)
    {
        if (dist > safeRadius) { EnterState(FishState.Patrol); return; }

        Vector3 fleeDir = (transform.position - player.position).normalized;
        fleeDir = SteerAroundObstacles(fleeDir);
        movement.applyMovement(fleeDir);

        Quaternion targetRot = Quaternion.LookRotation(fleeDir, Vector3.up);
        movement.applyRotation(targetRot);
    }

    void EnterState(FishState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case FishState.Idle:
                stateTimer = Random.Range(idleMinDuration, idleMaxDuration);
                break;
            case FishState.Patrol:
                stateTimer = Random.Range(3f, 7f);
                patrol.PickNewTarget(transform.position);
                break;
        }
    }

    Vector3 SteerAroundObstacles(Vector3 desiredDir)
    {
        if (Physics.Raycast(transform.position, desiredDir, out RaycastHit hit, 2f))
        {
            Vector3 deflected = Vector3.Reflect(desiredDir, hit.normal);
            return deflected.normalized;
        }
        return desiredDir;
    }
}

public enum FishState { Idle, Patrol, Flee }


