using UnityEngine;

[CreateAssetMenu(fileName = "fishData", menuName = "Scriptable Objects/fishData")]
public class fishData : ScriptableObject
{
    public string fishName;
    public Sprite fishImage;
    [TextArea] public string fishFact;

}
