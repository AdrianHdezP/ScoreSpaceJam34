using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class CarSetup
{
    public float maxSpeed;
    public float maxSpeedLerp;
    [Space]
    public float acelerationForce;
    public float acelerationForceLerp;
    [Space]
    public float steringForce;
    public float steringForceLerp;
    [Space]
    public float drifForce;
    public float drifForceLerp;
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BallController ball;

    [Header("Visuals")]
    [SerializeField] private Transform model;
    [SerializeField] private Animator modelAnim;

    [Header("Setup")]
    [SerializeField] private InputActionReference acelerationInput;
    [SerializeField] private InputActionReference steringInput;
    [SerializeField] private InputActionReference brakeInput;
    private float acelerationInputValue;
    private float steringInputValue;
    private float brakeInputValue;

    [Header("Settings")]
    [SerializeField] private CarSetup carSetup;
    [SerializeField] private CarSetup driftingCarSetup;

    Quaternion modelRot;

    private float maxSpeed;
    private float acelerationForce;
    private float steringForce;
    private float drift;
    private float rotationAngle;
    private float velocityVsUp;
    private bool isBracking;
    private float bracckingTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ball = FindFirstObjectByType<BallController>();

        modelRot = model.localRotation;
    }

    private void Start()
    {
        maxSpeed = carSetup.maxSpeed;
        acelerationForce = carSetup.acelerationForce;
        steringForce = carSetup.steringForce;
        drift = carSetup.drifForce;
    }

    private void AssignInputs()
    {
        acelerationInputValue = acelerationInput.action.ReadValue<float>();
        steringInputValue = steringInput.action.ReadValue<float>();
        brakeInputValue = brakeInput.action.ReadValue<float>();
    }

    private void Update()
    {
        AssignInputs();
        HandleBrake();

        VisualControl();
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
        Vector2 engineForceVector = transform.up * acelerationInputValue * acelerationForce;
        rb.AddForce(engineForceVector * rb.mass, ForceMode2D.Force);
    }

    private void ManageDrag()
    {
       if (acelerationInputValue == 0)
           rb.linearDamping = Mathf.Lerp(rb.linearDamping, 3f, Time.fixedDeltaTime * 4.5f);
       else
           rb.linearDamping = 0;
    }

    private void ApplyStering()
    {
        float minSpeed = rb.linearVelocity.magnitude / maxSpeed;
        minSpeed = Mathf.Clamp01(minSpeed);

        rotationAngle -= steringInputValue * steringForce * minSpeed;
        rb.MoveRotation(rotationAngle);
    }

    private void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);

        rb.linearVelocity = forwardVelocity + rightVelocity * drift;
    }

    public float GetLateralvelocity() => Vector2.Dot(transform.right, rb.linearVelocity);

    public bool IsTireScreeching(out float lateralVelocity, out bool isBracking)
    {
        lateralVelocity = GetLateralvelocity();
        isBracking = false;

        //If we are moving forward and player is hiting the brake
        if (acelerationInputValue < 0 && velocityVsUp > 0)
        {
            isBracking = true;
            return true;    
        }

        // ¡If we have a lot of side movement
        if (Mathf.Abs(GetLateralvelocity()) > 4f)
            return true;

        return false;
    }

    private void HandleBrake()
    {
        if (brakeInputValue > 0 && !isBracking)
        {
            isBracking = true;
        }
        else if (brakeInputValue <= 0 && isBracking)
        {
            isBracking = false;

            maxSpeed = maxSpeed + ball.combo * 5f;
            rb.AddForce(transform.up * rb.mass * ball.combo * 5f , ForceMode2D.Impulse);
        }

        LerpValues();
    }

    private void LerpValues()
    {
        if (isBracking)
        {
            maxSpeed = Mathf.MoveTowards(maxSpeed, driftingCarSetup.maxSpeed, driftingCarSetup.maxSpeedLerp * Time.deltaTime);
            drift = Mathf.MoveTowards(drift, driftingCarSetup.drifForce, driftingCarSetup.drifForceLerp * Time.deltaTime);
            steringForce = Mathf.MoveTowards(steringForce, driftingCarSetup.steringForce, driftingCarSetup.steringForceLerp * Time.deltaTime);
            acelerationForce = Mathf.MoveTowards(acelerationForce, driftingCarSetup.acelerationForce, driftingCarSetup.acelerationForceLerp * Time.deltaTime);
        }
        else
        {
            maxSpeed = Mathf.MoveTowards(maxSpeed, carSetup.maxSpeed, carSetup.maxSpeedLerp * Time.deltaTime);
            drift = Mathf.MoveTowards(drift, carSetup.drifForce, carSetup.drifForceLerp * Time.deltaTime);
            steringForce = Mathf.MoveTowards(steringForce, carSetup.steringForce, carSetup.steringForceLerp * Time.deltaTime);
            acelerationForce = Mathf.MoveTowards(acelerationForce, carSetup.acelerationForce, carSetup.acelerationForceLerp * Time.deltaTime);
        }                 
    }

    void VisualControl()
    {
        float angle = Mathf.Clamp(GetLateralvelocity() * 15f, -35, 35);
        model.localRotation = modelRot * Quaternion.AngleAxis(angle, -Vector3.right);

        if (acelerationInputValue != 0) modelAnim.SetBool("Move", true);
        else modelAnim.SetBool("Move", false);
    }
}
