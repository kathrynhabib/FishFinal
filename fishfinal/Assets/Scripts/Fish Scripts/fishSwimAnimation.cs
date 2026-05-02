using UnityEngine;

public class fishSwimAnimation : MonoBehaviour
{
    [Header("Wiggle")]
    public float wiggleSpeed = 3f;
    public float wiggleAmount = 15f;

    [Header("Bob")]
    public float bobSpeed = 1.5f;
    public float bobAmount = 0.05f;

    [Header("Clownfish Bounce")]
    public bool hasBouncySwim = false;
    public float bounceAmount = 0.08f;
    public float bounceSpeed = 4f;

    [Header("Tang Glide")]
    public bool hasGlide = false;
    public float glideWiggleAmount = 8f;

    private Rigidbody rb;
    private Vector3 startLocalPos;
    private float wiggleTimer;
    private float bobTimer;
    private float bounceTimer;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        float speed = rb != null ? rb.linearVelocity.magnitude : 0f;
        float speedRatio = Mathf.Clamp01(speed / 2f);

        AnimateWiggle(speedRatio);
        AnimateBob(speedRatio);

        if (hasBouncySwim) AnimateBounce(speedRatio);
        if (hasGlide)      AnimateGlide(speedRatio);
    }

    void AnimateWiggle(float speedRatio)
    {
        float currentWiggleAmount = hasGlide ? glideWiggleAmount : wiggleAmount;

        wiggleTimer += Time.deltaTime * wiggleSpeed * (0.5f + speedRatio);
        float wiggle = Mathf.Sin(wiggleTimer) * currentWiggleAmount * (0.3f + speedRatio);
        transform.localRotation = Quaternion.Euler(0, wiggle, 0);
    }

    void AnimateBob(float speedRatio)
    {
        bobTimer += Time.deltaTime * bobSpeed;
        float bob = Mathf.Sin(bobTimer) * bobAmount;
        transform.localPosition = startLocalPos + new Vector3(0, bob, 0);
    }

    void AnimateBounce(float speedRatio)
    {
        bounceTimer += Time.deltaTime * bounceSpeed * speedRatio;
        float bounce = Mathf.Sin(bounceTimer) * bounceAmount * speedRatio;
        transform.localPosition += new Vector3(0, bounce, 0);
    }

    void AnimateGlide(float speedRatio)
    {
        float tilt = Mathf.Sin(wiggleTimer * 0.5f) * 5f * speedRatio;
        transform.localRotation *= Quaternion.Euler(0, 0, tilt);
    }
}