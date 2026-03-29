using UnityEngine;

public class fishMovement : MonoBehaviour
{

    public float acceleration = 10;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Debug.Log("Velocity: " + rb.linearVelocity);
    }

    public void applyMovement(Vector3 direction)
    {
        if (direction.magnitude > 0.01f)
        {
            Vector3 accel = direction * Time.deltaTime * acceleration;
            rb.linearVelocity += accel;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
        
    }
}
