using UnityEngine;

public class EncyclopediaManagerScript : MonoBehaviour
{
    [SerializeField] public GameObject EncyclopediaEntry;
    [SerializeField] public Transform Content;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        printEntries();
    }

    // Update is called once per frame
    void Update()
    {
        
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
