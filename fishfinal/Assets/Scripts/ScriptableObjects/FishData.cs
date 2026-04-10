using UnityEngine;


[CreateAssetMenu(fileName = "New Fish Data", menuName = "Fish Game/Fish Data", order = 1)]
public class FishData : ScriptableObject
{
    [field:SerializeField] public string fishName {get; private set;}
    [field:SerializeField] public string fishFact {get; private set;}
    [field:SerializeField] public Sprite fishImage {get; private set;}

    [Header("Model")]
    public GameObject modelPrefab;

    [Header("Player Movement")]
    public float acceleration;
    public float turnSpeed;
    public float slowingSpeed;
    public float maxSpeed;

    //[Header("AI Movement")]
    //public fishBehavior behaviorType;

    [Header("Collider")]
    public float colliderRadius;
}
