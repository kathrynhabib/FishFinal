using UnityEngine;
using System.Collections.Generic;   

public class FishDiscoveryManager : MonoBehaviour
{

// Singleton that tracks which fish have been discovered and notifies UI.
// HashSet reference: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1

    public static FishDiscoveryManager Instance { get; private set; }

    public FishInfoPopup infoPopup;
    public FishCounter counter;
    private HashSet<FishData> discovered = new HashSet<FishData>();

    public int totalFish = 10;   // REMEMBER TO CHANGE 

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

            if (EncyclopediaManagerScript.Instance != null) EncyclopediaManagerScript.Instance.printEntries();
            else Debug.LogError("EncyclopediaManagerScript.Instance is null!");
        }
    }

    public HashSet<FishData> GetDiscoveredFish()
    {
        return discovered;
    }
}
