using Unity.VisualScripting;
using UnityEngine;

public class TransformAbility : MonoBehaviour, ISpecialAbility
{
    [SerializeField] private GameObject[] models;
    [SerializeField] private float coolDown = 5f;

    private int currentIdx = 0;
    private float cooldownTimer;
    private Rigidbody rb;

    public bool IsReady => cooldownTimer <= 0f;
    public float Cooldown => coolDown;

    private void Awake()
    {
        for (int i = 0; i < models.Length; i++)
        {
            models[i].SetActive(i == 0);
        }
    }
    private void Update()
    {
        if (coolDown > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
    public void Activate()
    {
        models[currentIdx].SetActive(false);
        currentIdx = (currentIdx + 1) % models.Length;
        models[currentIdx].SetActive(true);
        cooldownTimer = coolDown;
    }

    public void Deactivate() { }

}
