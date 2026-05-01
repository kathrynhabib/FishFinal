using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    public static InstructionsManager Instance { get; private set; }

    [SerializeField] private GameObject instructionsPanel;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        instructionsPanel.SetActive(false);
    }

    public void Show()
    {
        instructionsPanel.SetActive(true);
        Time.timeScale = 0f; 
    }
    void Update()
    {
        if (instructionsPanel.activeSelf && Input.anyKeyDown)
        {
            instructionsPanel.SetActive(false);
            Time.timeScale = 1f; 
        }
    }
}