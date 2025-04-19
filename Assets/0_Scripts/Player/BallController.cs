using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController playerSC;
    private DeliveryManager deliveryManager;

    [Header("Setup")]
    [SerializeField] GameObject firePrefab;
    [SerializeField] GameObject slimePrefab;
    [SerializeField] GameObject protectionPrefab;
    private DeliveryType currentDeliveryType;

    [Header("Settings")]
    [SerializeField] private Transform model;
    [SerializeField] private float fireEmissionThreshold;
    [SerializeField] private float speedBoostThreshold;
    [SerializeField] private float damageThreshold;
    [SerializeField] private float spawnDistance;

    float horizontalVelocity;
    Vector2? lastPos;
    Quaternion modelRot; 

    public int combo { private set; get; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSC = FindFirstObjectByType<PlayerController>();
        deliveryManager = FindFirstObjectByType<DeliveryManager>();

        modelRot = model.localRotation;
    }
    private void Update()
    {
        currentDeliveryType = deliveryManager.currentDeliveryType;

        horizontalVelocity = Vector2.Dot(transform.right, rb.linearVelocity);

        if (Mathf.Abs(horizontalVelocity) > fireEmissionThreshold && (lastPos == null || Vector2.Distance(transform.position, (Vector2) lastPos) >= spawnDistance))
        {
            lastPos = transform.position;

            if (currentDeliveryType == DeliveryType.Fire)
                Instantiate(firePrefab, transform.position, Quaternion.identity);
            else if (currentDeliveryType == DeliveryType.Slime)
                Instantiate(slimePrefab, transform.position, Quaternion.identity);
            else if (currentDeliveryType == DeliveryType.Protection)
                Instantiate(protectionPrefab, transform.position, Quaternion.identity);
        }

        if (Mathf.Abs(horizontalVelocity) < fireEmissionThreshold) 
            lastPos = null;

        if (Mathf.Abs(playerSC.GetLateralvelocity()) < speedBoostThreshold)
            combo = 0;

        VisualControl();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Damageable damagable) && collision.relativeVelocity.magnitude >= damageThreshold)
        {
            damagable.RecieveDamage(10, -collision.contacts[0].normal * collision.relativeVelocity.magnitude, true);

            if (Mathf.Abs(playerSC.GetLateralvelocity()) > speedBoostThreshold) 
                combo++;
        }
    }

    void VisualControl()
    {
        float angle = Mathf.Clamp(horizontalVelocity * 15f, -15, 15);
        model.localRotation = modelRot * Quaternion.AngleAxis(angle, -Vector3.right);
    }
}
