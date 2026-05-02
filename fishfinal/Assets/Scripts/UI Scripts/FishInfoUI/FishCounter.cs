
using UnityEngine;
using TMPro;    

public class FishCounter : MonoBehaviour
{

    public TMP_Text counterText;    

    void Start()
    {
        //counterText.text = "1/0";
    }   

    public void UpdateCount(int current, int total)
    {
        counterText.text = current + "/" + total;
    }
}