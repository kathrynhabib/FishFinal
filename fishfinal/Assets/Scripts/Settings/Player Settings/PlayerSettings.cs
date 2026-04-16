using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSettings : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject settingsPopup;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityLabel;

    [Header("Sensitivity")]
    [SerializeField] private float minSensitivity = 0.5f;
    [SerializeField] private float maxSensitivity = 10f;
    [SerializeField] private float defaultSensitivity = 3f;

    public static float currentSensitivity;

    void Start()
    {
        settingsPopup.SetActive(false);

        sensitivitySlider.minValue = minSensitivity;
        sensitivitySlider.maxValue = maxSensitivity;
        sensitivitySlider.value = defaultSensitivity;
        currentSensitivity = defaultSensitivity;
        sensitivityLabel.text = $"Sensitivity: {defaultSensitivity:F1}";

        sensitivitySlider.onValueChanged.AddListener(OnSliderChanged);
    }

    public void OpenSettings()
    {
        settingsPopup.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseSettings()
    {
        settingsPopup.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnSliderChanged(float value)
    {
        currentSensitivity = value;
        sensitivityLabel.text = $"Sensitivity: {value:F1}";
    }
}