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
    private float velocityX; 
    private float velocityY;
    private float velocityZ;

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

        if (Mathf.Abs(localInput.x) > 0.01f)
        {
            velocityX += localInput.x * acceleration * Time.deltaTime;
        }
        else velocityX *= slowingSpeed;

        if (Mathf.Abs(localInput.y) > 0.01f)
        {
            velocityY += localInput.y * acceleration * Time.deltaTime;
        }
        else velocityY *= slowingSpeed;

        if (Mathf.Abs(localInput.z) > 0.01f)
        {
            velocityZ += localInput.z * acceleration * Time.deltaTime;
        }
        else velocityZ *= slowingSpeed;

        velocityX = Mathf.Clamp(velocityX, -maxSpeed, maxSpeed);
        velocityY = Mathf.Clamp(velocityY, -maxSpeed, maxSpeed);
        velocityZ = Mathf.Clamp(velocityZ, -maxSpeed, maxSpeed);

        Vector3 localVelocity = new Vector3(velocityX, velocityY, velocityZ);
        Vector3 worldVelocity = transform.TransformDirection(localVelocity); 
        
        rb.MovePosition(rb.position + worldVelocity * Time.deltaTime);

    }

    public void applyRotation(Quaternion targetRotation)
    {
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}
