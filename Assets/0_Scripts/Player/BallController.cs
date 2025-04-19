using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] GameObject firePrefab;
    [SerializeField] Transform model;
    [SerializeField] float fireEmissionThreshold;
    [SerializeField] float speedBoostThreshold;
    [SerializeField] float damageThreshold;
    [SerializeField] float spawnDistance;

    float horizontalVelocity;

    PlayerController playerSC;
    Rigidbody2D rb;
    Vector2? lastPos;
    Quaternion modelRot;

    public int combo { private set; get; }

    private void Awake()
    {
        playerSC = FindFirstObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        modelRot = model.localRotation;
    }
    private void Update()
    {
        horizontalVelocity = Vector2.Dot(transform.right, rb.linearVelocity);

        if (Mathf.Abs(horizontalVelocity) > fireEmissionThreshold && (lastPos == null || Vector2.Distance(transform.position, (Vector2) lastPos) >= spawnDistance))
        {
            lastPos = transform.position;
            Instantiate(firePrefab, transform.position, Quaternion.identity);
        }
        if (Mathf.Abs(horizontalVelocity) < fireEmissionThreshold) lastPos = null;

        if (Mathf.Abs(playerSC.GetLateralvelocity()) < speedBoostThreshold)
        {
            combo = 0;
        }

        VisualControl();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Damageable damagable) && collision.relativeVelocity.magnitude >= damageThreshold)
        {
<<<<<<< Updated upstream
            damagable.RecieveDamage(10, -collision.contacts[0].normal * collision.relativeVelocity.magnitude, true);
=======
            damagable.RecieveDamage(10, -collision.contacts[0].normal * collision.relativeVelocity.magnitude);

            if (Mathf.Abs(playerSC.GetLateralvelocity()) > speedBoostThreshold) combo++;
>>>>>>> Stashed changes
        }
    }


    void VisualControl()
    {
        float angle = Mathf.Clamp(horizontalVelocity * 15f, -15, 15);
        model.localRotation = modelRot * Quaternion.AngleAxis(angle, -Vector3.right);
    }
}
