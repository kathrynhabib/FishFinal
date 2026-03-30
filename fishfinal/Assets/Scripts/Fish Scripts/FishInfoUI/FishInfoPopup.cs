
using UnityEngine;
using UnityEngine.UI;   
using TMPro;            

public class FishInfoPopup : MonoBehaviour
// Shows a UI panel with the fish's image and fact when you discover a new species.

{
    public CanvasGroup canvasGroup;
    public Image      fishImage;
    public TMP_Text   fishNameText;
    public TMP_Text   fishFactText;
    public Button     closeButton;

    
    void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        closeButton.onClick.AddListener(Hide);
    }

    [ContextMenu("Test Show")]  // for testing in inspector
    public void Show(fishData data)
    {
        fishImage.sprite = data.fishImage;
        fishNameText.text = data.fishName;
        fishFactText.text = data.fishFact;

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        Time.timeScale = 0f;    
    }

    [ContextMenu("Test Hide")]  // for testing in inspector
    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        Time.timeScale = 1f;    // resume  game
    }
}