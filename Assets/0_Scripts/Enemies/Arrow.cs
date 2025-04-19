using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float arrowSpeed;
    [SerializeField] float knockbackForce;
    [SerializeField] int impactDamage;
    [SerializeField] LayerMask activationLayer;
    [SerializeField] ParticleSystem particlePrefab;

    public EnemyRanged shooter;

    bool impacted;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.linearVelocity = transform.up * arrowSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((activationLayer & (1 << collision.gameObject.layer)) != 0 && !impacted && (!collision.TryGetComponent(out EnemyRanged enemy) || enemy != shooter))
        {
            if (collision.TryGetComponent(out Damageable damageable))
            {
                damageable.RecieveDamage(impactDamage, (collision.transform.position - transform.position).normalized * knockbackForce);
            }

            impacted = true;
            Instantiate(particlePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
