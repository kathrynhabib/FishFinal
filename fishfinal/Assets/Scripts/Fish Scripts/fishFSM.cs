using UnityEngine;
using FishAlive;

public class fishFSM : MonoBehaviour

{
    [Header("Detection")]
    public float detectionRadius = 8f;
    public float safeRadius = 14f;

    [Header("Idle")]
    public float idleMinDuration = 1.5f;
    public float idleMaxDuration = 4f;

    [HideInInspector] public FishState currentState = FishState.Idle;

    private FishMotion fishMotion;
    private fishPatrol patrol;
    private Transform player;
    private float stateTimer;

    private GameObject targetProxy;

    void Awake()
    {
        fishMotion = GetComponent<FishMotion>();
        patrol = GetComponent<fishPatrol>();

        targetProxy = new GameObject("FSM_TargetProxy");
    }

    void Start()
    {
        fishMotion.target = targetProxy;
        fishMotion.SetReachMode(ReachMode.Wander);
        EnterState(FishState.Idle);
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
        fishMotion.SetAutoMotion(false);
        stateTimer -= Time.deltaTime;

        if (dist < detectionRadius) { EnterState(FishState.Flee);   return; }
        if (stateTimer <= 0f)       { EnterState(FishState.Patrol); return; }
    }

    void UpdatePatrol(float dist)
    {
        if (dist < detectionRadius) { EnterState(FishState.Flee); return; }

        targetProxy.transform.position = patrol.GetPatrolTarget(transform.position);
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f) { EnterState(FishState.Idle); return; }
    }

    void UpdateFlee(float dist)
    {
        if (dist > safeRadius) { EnterState(FishState.Patrol); return; }

        Vector3 fleePos = transform.position + (transform.position - player.position).normalized * 1f;
        targetProxy.transform.position = fleePos;
    }

    void EnterState(FishState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case FishState.Idle:
                stateTimer = Random.Range(idleMinDuration, idleMaxDuration);
                fishMotion.SetAutoMotion(false);
                break;
            case FishState.Patrol:
                stateTimer = Random.Range(3f, 7f);
                fishMotion.SetAutoMotion(true);
                fishMotion.SetReachMode(ReachMode.Wander);
                patrol.PickNewTarget(transform.position);
                break;
            case FishState.Flee:
                fishMotion.SetAutoMotion(true);
                fishMotion.SetReachMode(ReachMode.Wander);
                break;
        }
    }

    void OnDestroy()
    {
        if (targetProxy != null) Destroy(targetProxy);
    }

}