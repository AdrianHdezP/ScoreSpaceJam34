using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;

    [Header("Setup")]
    [SerializeField] private InputActionReference acelerationInput;
    [SerializeField] private InputActionReference steringInput;
    private float acelerationInputValue;
    private float steringInputValue;

    [Header("Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acelerationFactor = 25f;
    [SerializeField] private float turnFactor = 2.5f;
    [SerializeField] private float drifFactor = 0.75f;
    private float rotationAngle;
    private float velocityVsUp;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void AssignInputs()
    {
        acelerationInputValue = acelerationInput.action.ReadValue<float>();
        steringInputValue = steringInput.action.ReadValue<float>();
    }

    private void Update()
    {
        AssignInputs();
    }

    private void FixedUpdate()
    {
       ApplyEngineForce();
       KillOrthogonalVelocity();
       ApplyStering();
    }

    private void ApplyEngineForce()
    {
        //Calculate how much "forward"
        velocityVsUp = Vector2.Dot(transform.up, rb.linearVelocity);

        //Limit forward velocity in max speed
        if (velocityVsUp > maxSpeed && acelerationInputValue > 0)
            return;

        //Limit backward velocity in 50% of max speed
        if (velocityVsUp < -maxSpeed * 0.5f && acelerationInputValue < 0)
            return;

        if (rb.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed && acelerationInputValue > 0)
            return;

        ManageDrag();

        //Create an apply force
        Vector2 engineForceVector = transform.up * acelerationInputValue * acelerationFactor;
        rb.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void ManageDrag()
    {
        if (acelerationInputValue == 0)
            rb.linearDamping = Mathf.Lerp(rb.linearDamping, 3f, Time.fixedDeltaTime * 3);
        else
            rb.linearDamping = 0;
    }

    private void ApplyStering()
    {
        float minSpeed = rb.linearVelocity.magnitude / 8;
        minSpeed = Mathf.Clamp01(minSpeed);

        rotationAngle -= steringInputValue * turnFactor * minSpeed;
        rb.MoveRotation(rotationAngle);
    }

    private void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);

        rb.linearVelocity = forwardVelocity + rightVelocity * drifFactor;
    }
}
