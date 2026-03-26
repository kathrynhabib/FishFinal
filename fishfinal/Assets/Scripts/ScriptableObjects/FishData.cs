using UnityEngine;


[CreateAssetMenu(fileName = "New Fish Data", menuName = "Fish Game/Fish Data", order = 1)]
public class FishData : ScriptableObject
{
    [field:SerializeField] public string FishName {get; private set;}
    [field:SerializeField] public string FishDescription {get; private set;}
    [field:SerializeField] public Sprite Icon {get; private set;}
}
