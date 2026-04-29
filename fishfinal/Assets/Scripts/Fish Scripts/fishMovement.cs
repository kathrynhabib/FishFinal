using UnityEngine;

public class fishMovement : MonoBehaviour
{

    // restrictions/ adaptations to movement would be passed in thru fishData
    // both ai behavior  and player input scripts would change positioning by calling applyMovement

    public float acceleration = 10;
    public float turnSpeed = 3f; // turning speed of the fish
    public float drag = 2f;
    public float maxSpeed = 1.5f; // should vary per fish

    public bool horizontalEnabled;
    public bool verticalEnabled;

    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = drag;
    }
    public void applyMovement(Vector3 inputDir)
    {
        Vector3 localInput = transform.InverseTransformDirection(inputDir);

        if (!horizontalEnabled)
            localInput.x = 0f;

        if (!verticalEnabled)
            localInput.y = 0f;

        if (localInput.magnitude > 0.01f)
        {
            Vector3 worldInput = transform.TransformDirection(localInput.normalized);
            rb.AddForce(worldInput * acceleration, ForceMode.Acceleration);
        }

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

    }

    public void applyRotation(Quaternion targetRotation)
    {
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}