using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class fishCameraController : MonoBehaviour
{
    // needs to set the target and radius of the camera on the current fish
    // must be called by the datacontroller when switching
    
    public CinemachineCamera cam;

    void Start()
    {
  
    }

    void Update()
    {
        
    }

    public void setTarget (Transform targetFish)
    {
        cam.Follow = targetFish;
    }

    public void setRadius (float camRadius)
    {
        cam.GetComponent<CinemachineOrbitalFollow>().Radius = camRadius;
    }

}
