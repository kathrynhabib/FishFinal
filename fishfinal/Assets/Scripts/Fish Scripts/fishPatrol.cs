using UnityEngine;

public class fishPatrol : MonoBehaviour

{
    [Header("Patrol Volume")]
    public Vector3 patrolCenter = Vector3.zero;
    public Vector3 patrolExtents = new Vector3(10, 4, 10);
    public float arrivalDistance = 1.5f;

    private Vector3 currentTarget;
    private bool hasTarget = false;

    public void PickNewTarget(Vector3 fromPosition)
    {
        currentTarget = patrolCenter + new Vector3(
            Random.Range(-patrolExtents.x, patrolExtents.x),
            Random.Range(-patrolExtents.y, patrolExtents.y),
            Random.Range(-patrolExtents.z, patrolExtents.z)
        );
        hasTarget = true;
    }

    public Vector3 GetPatrolTarget(Vector3 currentPos)
    {
        if (!hasTarget) PickNewTarget(currentPos);

        if (Vector3.Distance(currentPos, currentTarget) < arrivalDistance)
            PickNewTarget(currentPos);

        return currentTarget;
    }

}