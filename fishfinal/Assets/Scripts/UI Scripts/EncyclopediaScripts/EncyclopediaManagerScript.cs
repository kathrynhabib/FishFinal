using UnityEngine;
using System.Linq;
//citation for linq: https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries

public class EncyclopediaManagerScript : MonoBehaviour
{
    public static EncyclopediaManagerScript Instance { get; private set; }

    [SerializeField] public GameObject EncyclopediaEntry;
    [SerializeField] public Transform Content;

    private Biome currentBiome = Biome.All;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start() { PrintEntries(); }
    void OnEnable() { PrintEntries(); }

    public void FilterByBiome(int biome)
    {
        currentBiome = (Biome)biome;
        PrintEntries();
    }

    #region claude generated method
    public void PrintEntries()
    {
        if (FishDiscoveryManager.Instance == null) return;

        foreach (Transform entry in Content)
            Destroy(entry.gameObject);

        var discovered = FishDiscoveryManager.Instance.GetDiscoveredFish();

        var filtered = currentBiome == Biome.All
            ? discovered
            : discovered.Where(f => f.biome == currentBiome);

        foreach (FishData fish in filtered)
        {
            GameObject entry = Instantiate(EncyclopediaEntry, Content);
            entry.GetComponent<EncyclopediaEntry>().Setup(fish);
        }
    }
    #endregion
}