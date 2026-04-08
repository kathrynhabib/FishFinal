using UnityEngine;

public class EncyclopediaManagerScript : MonoBehaviour
{
    public static EncyclopediaManagerScript Instance { get; private set; }
    
    [SerializeField] public GameObject EncyclopediaEntry;
    [SerializeField] public Transform Content;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void OnEnable()
    {
        printEntries();
    }

    public void printEntries()
    {
        foreach(Transform entry in Content )
        {
            Destroy(entry.gameObject);
        }

        var discoveredFish = FishDiscoveryManager.Instance.GetDiscoveredFish();

        foreach (FishData fish in discoveredFish)
        {
            GameObject entry = Instantiate(EncyclopediaEntry, Content);
            entry.GetComponent<EncyclopediaEntry>().Setup(fish);
        }
    }
}
