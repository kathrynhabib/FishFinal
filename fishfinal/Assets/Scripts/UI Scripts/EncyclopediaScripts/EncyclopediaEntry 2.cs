using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EncyclopediaEntry : MonoBehaviour
{
    public Image      fishImage;
    public TMP_Text   fishNameText;
    public TMP_Text   fishFactText;

    public void Setup(FishData data)
    {
        fishImage.sprite = data.fishImage;
        fishNameText.text = data.fishName;
        fishFactText.text = data.fishFact;
    }
}
