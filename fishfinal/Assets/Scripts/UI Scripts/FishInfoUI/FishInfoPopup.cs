using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishInfoPopup : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Image fishImage;
    public TMP_Text fishNameText;
    public TMP_Text fishFactText;
    public Button closeButton;

    void Start()
    {
        Time.timeScale = 1f;
        SetVisible(false);
        closeButton.onClick.AddListener(Hide);
    }

    [ContextMenu("Test Show")]
    public void Show(FishData data)
    {
        if (data == null) { Debug.LogError("Show() called with null FishData!"); return; }

        if (fishImage != null && data.fishImage != null)
            fishImage.sprite = data.fishImage;

        fishNameText.text = data.fishName;
        fishFactText.text = data.fishFact;

        SetVisible(true);
        Time.timeScale = 0f;
    }

    [ContextMenu("Test Hide")]
    public void Hide()
    {
        SetVisible(false);
        Time.timeScale = 1f;
    }

    void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }
}