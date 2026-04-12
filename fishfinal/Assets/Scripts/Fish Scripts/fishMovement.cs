using UnityEngine;

public class fishMovement : MonoBehaviour
{

    // restrictions/ adaptations to movement would be passed in thru fishData
    // both ai behavior  and player input scripts would change positioning by calling applyMovement

    public float acceleration = 10;
    public float turnSpeed = 3f; // turning speed of the fish
    public float slowingSpeed = .99f;
    public float maxSpeed = 1.5f; // should vary per fish

    public bool horizontalEnabled;
    public bool verticalEnabled;

    private Rigidbody rb;
    private Vector3 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void applyMovement(Vector3 inputDir)
    {
        Vector3 localInput = transform.InverseTransformDirection(inputDir);

        if (!horizontalEnabled)
            localInput.x = 0f;

        if (!verticalEnabled)
            localInput.y = 0f;

        inputDir = transform.TransformDirection(localInput);

        if (inputDir.magnitude > 0.01f)
        {
            inputDir.Normalize();

            velocity += inputDir * acceleration * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }
        else // smoothing slowing
        {
            velocity *= slowingSpeed;
        }
        rb.MovePosition(rb.position + velocity * Time.deltaTime);

    }

    public void applyRotation(Quaternion targetRotation)
    {
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}
