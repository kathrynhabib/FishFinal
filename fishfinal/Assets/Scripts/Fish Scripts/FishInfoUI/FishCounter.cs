
using UnityEngine;
using TMPro;    // use Text if you don't have TextMeshPro

public class FishCounter : MonoBehaviour
{// Updates the "X/Y fish discovered" text in the top of the screen.

    public TMP_Text counterText;    

    void Start()
    {
        //source for find first object of type: https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Object.FindFirstObjectByType.html
        counterText.text = "0/" + FindFirstObjectByType<FishDiscoveryManager>().totalFish;
    }

    // called by FishDiscoveryManager each time a new fish is found
    public void UpdateCount(int current, int total)
    {
        counterText.text = current + "/" + total;
    }
}