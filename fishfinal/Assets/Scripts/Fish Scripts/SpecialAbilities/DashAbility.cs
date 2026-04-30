using Unity.VisualScripting;
using UnityEngine;

public class DashAbility : MonoBehaviour, ISpecialAbility
{
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float coolDown = 5f;

    private float cooldownTimer;
    private Rigidbody rb;

    public bool IsReady => cooldownTimer <= 0f;
    public float Cooldown => coolDown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(coolDown > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
    public void Activate()
    {
        rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
        cooldownTimer = coolDown;
    }

    public void Deactivate() { }

}
