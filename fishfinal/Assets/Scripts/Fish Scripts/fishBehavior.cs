using FishAlive;
using UnityEngine;
using UnityEngine.InputSystem;

public class fishBehavior : MonoBehaviour
{
    [Header("Detection")]
    public float detectionRadius = 6f;
    public float safeRadius = 8f;

    [Header("Idle")]
    public float idleMinDuration = 0.5f;
    public float idleMaxDuration = 2f;

    private FishMotion motion;
    private fishPatrol patrol;
    private Transform player;
    private float stateTimer;
    private GameObject targetProxy;
    private FishState currentState = FishState.Idle;

    void Awake()
    {
        motion = GetComponent<FishMotion>();
        patrol = GetComponent<fishPatrol>();

        targetProxy = new GameObject("FSM_TargetProxy");
        motion.target = targetProxy;
    }

    public void enable()
    {
        enabled = true;
        motion.SetAutoMotion(true);
        motion.SetReachMode(ReachMode.Wander);
        motion.target = targetProxy;
        EnterState(FishState.Patrol);
        Debug.Log("AI enabled on: " + gameObject.name);
    }

    public void disable()
    {
        enabled = false;                  // stops Update() from running
        motion.SetAutoMotion(false);      // stops FishMotion moving
        Debug.Log("AI disabled on: " + gameObject.name);
    }

    void Update()
    {
        if (PlayerFishTracker.Current != null)
            player = PlayerFishTracker.Current;

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
        motion.SetAutoMotion(false);
        stateTimer -= Time.deltaTime;

        if (dist < detectionRadius) { EnterState(FishState.Flee);   return; }
        if (stateTimer <= 0f)       { EnterState(FishState.Patrol); return; }
    }

    void UpdatePatrol(float dist)
    {
        if (dist < detectionRadius) { EnterState(FishState.Flee); return; }

        Vector3 patrolTarget = patrol.GetPatrolTarget(transform.position);
        targetProxy.transform.position = SteerAroundObstacles(patrolTarget);

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f) { EnterState(FishState.Idle); return; }
    }

    void UpdateFlee(float dist)
    {
        if (dist > safeRadius) { EnterState(FishState.Patrol); return; }

        Vector3 fleePos = transform.position + (transform.position - player.position).normalized * 2f;
        targetProxy.transform.position = SteerAroundObstacles(fleePos);
    }

    void EnterState(FishState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case FishState.Idle:
                stateTimer = Random.Range(idleMinDuration, idleMaxDuration);
                motion.SetAutoMotion(false);
                break;
            case FishState.Patrol:
                stateTimer = Random.Range(3f, 7f);
                motion.SetAutoMotion(true);
                motion.SetReachMode(ReachMode.Wander);
                patrol.PickNewTarget(transform.position);
                break;
            case FishState.Flee:
                motion.SetAutoMotion(true);
                motion.SetReachMode(ReachMode.Wander);
                break;
        }
    }

    Vector3 SteerAroundObstacles(Vector3 desiredPosition)
    {
        Vector3 dir = (desiredPosition - transform.position).normalized;
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 2f))
        {
            Vector3 deflected = Vector3.Reflect(dir, hit.normal);
            return transform.position + deflected * 2f;
        }
        return desiredPosition;
    }

    void OnDestroy()
    {
        if (targetProxy != null) Destroy(targetProxy);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, safeRadius);
    }
}

public enum FishState { Idle, Patrol, Flee }


/*public class fishBehavior : MonoBehaviour // can use this script for setting the presets used by fishMotion and enable it in fishDataController
    // separate from fishDataController so we can ahve varying presets for each fish type
{
    // enable and set variables of fishMotion, fishFSM, fishPatrol
    public fishFSM fsm;
    public fishPatrol patrol;
    public FishMotion motion;

    // switchToPlayer method to disable use of fishMotion
    void Awake()
    {

        fsm = GetComponent<fishFSM>();
        patrol = GetComponent<fishPatrol>();
        motion = GetComponent<FishMotion>();

    }

    public void enable()
    {
        //Debug.Log("fsmmmmm " + fsm.enabled);
        fsm.enabled = true;
        patrol.enabled = true;
        motion.enabled = true;

        // can set custom variables here
    }

    public void disable()
    {
        fsm.enabled = false;
        patrol.enabled = false;
        motion.enabled = false;
    }
}
*/
