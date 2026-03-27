using UnityEngine;

public class EncyclopediaController : MonoBehaviour
{

    [SerializeField] public GameObject EncyclopediaPanel;
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isOpen)
            {
                closeEncyclopedia();
            }
            else
            {
                openEncyclopedia();
            }
        }
    }

    public void openEncyclopedia()
    {
        EncyclopediaPanel.SetActive(true);
        Time.timeScale = 0f;
        isOpen = true;
    }

    public void closeEncyclopedia()
    {
        EncyclopediaPanel.SetActive(false);
        Time.timeScale = 1f;
        isOpen = false;
    }
}
