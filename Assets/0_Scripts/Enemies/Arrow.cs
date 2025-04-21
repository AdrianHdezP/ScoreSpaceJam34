using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float arrowSpeed;
    [SerializeField] float knockbackForce;
    [SerializeField] float explosionRange = 2.7f;
    [SerializeField] int impactDamage;
    [SerializeField] LayerMask activationLayer;
    [SerializeField] ParticleSystem particlePrefab;
    [SerializeField] ParticleSystem particlesFollow;

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

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRange);

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out Damageable damageable))
                {
                    damageable.RecieveDamage(impactDamage, (collider.transform.position - transform.position).normalized * knockbackForce, false);
                }
            }

            impacted = true;

            particlesFollow.transform.SetParent(null);
            particlesFollow.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            Destroy(particlesFollow.gameObject, 2);

            Instantiate(particlePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


}
