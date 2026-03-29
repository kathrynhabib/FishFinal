// Shows a UI panel with the fish's image and fact when you discover a new species.

using UnityEngine;
using UnityEngine.UI;   
using TMPro;            

public class FishInfoPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public Image      fishImage;
    public TMP_Text   fishNameText;
    public TMP_Text   fishFactText;
    public Button     closeButton;

    void Start()
    {
        popupPanel.SetActive(false);

        closeButton.onClick.AddListener(Hide);
    }

    public void Show(fishData data)
    {
        fishImage.sprite = data.fishImage;
        fishNameText.text = data.fishName;
        fishFactText.text = data.fishFact;

        popupPanel.SetActive(true);
        Time.timeScale = 0f;    
    }

    public void Hide()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f;    // resume  game
    }
}