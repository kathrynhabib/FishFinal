using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using UnityEditor.SettingsManagement;

public class fishCameraController : MonoBehaviour
{
    // needs to set the target and radius of the camera on the current fish
    // must be called by the datacontroller when switching

    // also controls mouse sensitivity thru gain
    
    public CinemachineCamera cam;
    private PlayerSettings settings;

    void Start()
    {
        settings = GetComponent<PlayerSettings>();
    }

    void Update()
    {
        setMouseSens();
    }

    public void setTarget (Transform targetFish)
    {
        cam.Follow = targetFish;
    }

    public void setRadius (float camRadius)
    {
        cam.GetComponent<CinemachineOrbitalFollow>().Radius = camRadius;
    }

    public void setMouseSens()
    {
        var axisController = cam.GetComponent<CinemachineInputAxisController>();

        axisController.Controllers[0].Input.Gain = Mathf.Sign(axisController.Controllers[0].Input.Gain) * PlayerSettings.currentSensitivity;
        axisController.Controllers[1].Input.Gain = Mathf.Sign(axisController.Controllers[1].Input.Gain) * PlayerSettings.currentSensitivity;
        
    }

}
