using System;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class CarSetup
{
    public float maxSpeed;
    public float acelerationForce;
    public float steringForce;
    public float drifForce;

    [HideInInspector] public float defaultMaxSpeed;
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

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
    }

    private void Start()
    {
        carSetup.defaultMaxSpeed = carSetup.maxSpeed;

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
        LerpVelocity();
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
            rb.linearDamping = Mathf.Lerp(rb.linearDamping, 3f, Time.fixedDeltaTime * 3);
        else
            rb.linearDamping = 0;
    }

    private void ApplyStering()
    {
        float minSpeed = rb.linearVelocity.magnitude / 8;
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

    private float GetLateralvelocity() => Vector2.Dot(transform.right, rb.linearVelocity);

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

            acelerationForce = driftingCarSetup.acelerationForce;
            steringForce = driftingCarSetup.steringForce;
            drift = driftingCarSetup.drifForce;
        }
        else if (brakeInputValue <= 0 && isBracking)
        {
            isBracking = false;

            carSetup.maxSpeed = 10;
            rb.linearVelocity = transform.up * 10;

            acelerationForce = carSetup.acelerationForce;
            steringForce = carSetup.steringForce;
            drift = carSetup.drifForce;
        }

        if (isBracking)
            maxSpeed = driftingCarSetup.maxSpeed;
        else
            maxSpeed = carSetup.maxSpeed;

    }

    private void LerpVelocity()
    {
        carSetup.maxSpeed = Mathf.MoveTowards(carSetup.maxSpeed, carSetup.defaultMaxSpeed, 50f * Time.deltaTime);
    }
}
