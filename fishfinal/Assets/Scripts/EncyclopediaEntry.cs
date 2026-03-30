using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EncyclopediaEntry : MonoBehaviour
{
    public Image      fishImage;
    public TMP_Text   fishNameText;
    public TMP_Text   fishFactText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(fishData data)
    {
        fishImage.sprite = data.fishImage;
        fishNameText.text = data.fishName;
        fishFactText.text = data.fishFact;
    }
}
