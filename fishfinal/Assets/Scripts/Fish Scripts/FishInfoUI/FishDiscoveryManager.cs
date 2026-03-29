using UnityEngine;
using System.Collections.Generic;   

public class FishDiscoveryManager : MonoBehaviour
{

// Singleton that tracks which fish have been discovered and notifies UI.
// HashSet reference: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1

    public static FishDiscoveryManager Instance { get; private set; }

    public FishInfoPopup infoPopup;
    public FishCounter counter;
    private HashSet<fishData> discovered = new HashSet<fishData>();

    public int totalFish = 2;   // REMEMBER TO CHANGE 

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    // when the player "becomes" a new fish.
    public void Discover(fishData data)
    {
        bool isNew = discovered.Add(data);

        if (isNew)
        {   //SOURCE: https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.messagebox.show?view=windowsdesktop-10.0
            infoPopup.Show(data); // show the info popup
            counter.UpdateCount(discovered.Count, totalFish);  
        }
    }
}
