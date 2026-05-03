using UnityEngine;


[CreateAssetMenu(fileName = "New Fish Data", menuName = "Fish Game/Fish Data", order = 1)]
public class FishData : ScriptableObject
{
    [field:SerializeField] public string fishName {get; private set;}
    [field:SerializeField] public string fishFact {get; private set;}
    [field:SerializeField] public Sprite fishImage {get; private set;}

    [Header("Biome")]
    public Biome biome;

    [Header("Model")]
    public GameObject[] modelPrefabs;

    [Header("Player Movement")]
    public float acceleration;
    public float turnSpeed;
    public float slowingSpeed;
    public float maxSpeed;
    public bool horizontalEnabled;
    public bool verticalEnabled;


    //[Header("AI Movement")]
    //public fishBehavior behaviorType;

    [Header("Collider")] // should prolly adapt this for options beyond a capsule for more complex creatures
    public float colliderRadius;

    [Header("Camera distance")]
    public float cameraRadius; 
}

public enum Biome { All, Reef, KelpForest, OpenOcean, DeepSea}
//outside so fishdata and encyclopedia can reference it without FishData.Biome reference
