using FishAlive;
using UnityEngine;
using UnityEngine.InputSystem;

public class fishBehavior : MonoBehaviour // can use this script for setting the presets used by fishMotion and enable it in fishDataController
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
