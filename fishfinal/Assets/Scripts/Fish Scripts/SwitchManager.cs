using UnityEngine;

public class SwitchManager : MonoBehaviour // singleton #fire
                                           // https://csharpindepth.com/articles/singleton
{
    public static SwitchManager Instance;
    private fishDataController currentPlayer;
    void Start()
    {
        
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        fishPlayerInput.OnSwitchRequested += TrySelectFish;
    }

    private void OnDisable()
    {
        fishPlayerInput.OnSwitchRequested -= TrySelectFish;
    }

    public void AssignPlayer(fishDataController fish)
    {
        currentPlayer = fish;

    }

    private void TrySelectFish()
    {
        if (currentPlayer == null || currentPlayer.gameObject == null)
        { 
            //Debug.Log("currentPlayer dead"); 
            return; 
        }

        if (SwitchConfirmPopup.Instance != null && SwitchConfirmPopup.Instance.IsOpen)
        {
            return;
        }

        Ray ray = currentPlayer.mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.SphereCast(ray, 1.5f, out RaycastHit hit, 100f))
        {
            fishDataController targetFish = hit.collider.GetComponentInParent<fishDataController>();

            if (targetFish != null && targetFish != currentPlayer && targetFish.tag == "fish")
            {
                targetFish.setHighlight(Color.white);
                //Debug.Log("making selected fish white");

                SwitchConfirmPopup.Instance.Show(targetFish.FishData.fishName,
                    () =>
                    {
                        //Debug.LogError("first selection screen triggered");
                        SwitchFish(targetFish);
                        //clearSelection();
                    },
                    () =>
                    {
                        //Debug.LogError("cancel screen triggered");
                        currentPlayer.clearSelection(targetFish);
                    }
                );
            }
        }
    }

    private void SwitchFish(fishDataController target)
    {
        fishCameraController camControl = currentPlayer.mainCamera.GetComponent<fishCameraController>();

        camControl.setTarget(target.thisPrefab.transform);
        camControl.setRadius(target.FishData.cameraRadius);

        currentPlayer.SetAI();
        target.SetPlayer();

        currentPlayer = target;
        target.unhighlight();

        FishDiscoveryManager.Instance.Discover(target.FishData);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer != null && !currentPlayer.gameObject.activeInHierarchy)
            Debug.Log("currentPlayer went dead this frame!");
    }

}
