using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;   

[System.Serializable]
public class RegionData
{   
    
    public string regionName;
    public List<FishData> requiredFish;
    public UnityEvent onCompleted;
    [HideInInspector] public bool unlocked = false;
    
}

public class FishDiscoveryManager : MonoBehaviour
{

// HashSet reference: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1

    public static FishDiscoveryManager Instance { get; private set; }

    public FishInfoPopup infoPopup;
    public FishCounter counter;
    private HashSet<FishData> discovered = new HashSet<FishData>();

    public int totalFish = 38;  

    #region Claude generated
    [Header("Regions")]
    public List<RegionData> regions;
    #endregion

    void Awake()
    {
        if (Instance == null) {
        Instance = this;
        DontDestroyOnLoad(gameObject); 
        } else {
        Destroy(gameObject);
    }
    }

    public void Discover(FishData data)
    {
        Debug.Log("Discover called with: " + data.fishName);
        bool isNew = discovered.Add(data);
         Debug.Log("Is new: " + isNew + " | Total discovered: " + discovered.Count);

        if (isNew)
        {
            if (infoPopup != null) infoPopup.Show(data);
            else Debug.LogError("infoPopup is null!");

            if (counter != null) counter.UpdateCount(discovered.Count, totalFish);
            else Debug.LogError("counter is null!");

            if (EncyclopediaManagerScript.Instance != null) EncyclopediaManagerScript.Instance.PrintEntries();
            else Debug.LogError("EncyclopediaManagerScript.Instance is null!");

            CheckAllRegions(); //Claude generated
        }
    }

    void CheckAllRegions()
    {
        foreach (RegionData region in regions)
        {
            if (region.unlocked) continue;
            if (region.requiredFish == null || region.requiredFish.Count == 0) continue;

            bool complete = true;
            foreach (FishData fish in region.requiredFish)
            {
                if (!discovered.Contains(fish))
                {
                    complete = false;
                    break;
                }
            }

            if (complete)
            {
                region.unlocked = true;
                Debug.Log(region.regionName + " complete!");
                region.onCompleted?.Invoke();
            }
        }
    }

    public HashSet<FishData> GetDiscoveredFish()
    {
        return discovered;
    }
}