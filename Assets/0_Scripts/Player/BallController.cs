using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] GameObject firePrefab;
    [SerializeField] float fireEmissionThreshold;
    [SerializeField] float damageThreshold;
    [SerializeField] float spawnDistance;

    Rigidbody2D rb;
    Vector2? lastPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (rb.linearVelocity.magnitude > fireEmissionThreshold && (lastPos == null || Vector2.Distance(transform.position, (Vector2) lastPos) >= spawnDistance))
        {
            lastPos = transform.position;
            Instantiate(firePrefab, transform.position, Quaternion.identity);
        }

        if (rb.linearVelocity.magnitude < fireEmissionThreshold) lastPos = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damagable) && collision.relativeVelocity.magnitude >= damageThreshold)
        {
            damagable.RecieveDamage(10, -collision.contacts[0].normal);
        }
    }
}
