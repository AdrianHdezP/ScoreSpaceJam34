using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] GameObject firePrefab;
    [SerializeField] float fireEmissionThreshold;
    [SerializeField] float damageThreshold;
    [SerializeField] float spawnDistance;

    [SerializeField] float horizontalVelocity;

    Rigidbody2D rb;
    Vector2? lastPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Damageable damagable) && collision.relativeVelocity.magnitude >= damageThreshold)
        {
            damagable.RecieveDamage(10, -collision.contacts[0].normal * collision.relativeVelocity.magnitude, true);
        }
    }
}
