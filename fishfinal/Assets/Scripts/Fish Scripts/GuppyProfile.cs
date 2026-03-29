using UnityEngine;
using FishAlive;

[RequireComponent(typeof(FishMotion))]
[RequireComponent(typeof(fishFSM))]
[RequireComponent(typeof(fishPatrol))]
public class GuppyProfile : MonoBehaviour
{
    void Awake()
    {
        var fsm = GetComponent<fishFSM>();
        fsm.detectionRadius = 6f;
        fsm.safeRadius      = 6f;
        fsm.idleMinDuration = 0.5f;
        fsm.idleMaxDuration = 2f;

        var patrol = GetComponent<fishPatrol>();
        patrol.patrolExtents   = new Vector3(5, 2, 5);
        patrol.arrivalDistance = 1f;
    }
}