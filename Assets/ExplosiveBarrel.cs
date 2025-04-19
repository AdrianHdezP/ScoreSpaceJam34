using UnityEngine;
using UnityEngine.UI;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] float detonationTimer;
    [SerializeField] float explosionRange;
    [SerializeField] float explosionImpulse;
    [SerializeField] float impulseMultiplier;
    [SerializeField] float explosionThreshold = 5;
    [SerializeField] LayerMask activationLayer;

    [SerializeField] ParticleSystem particlePrefab;

    bool activated;
    float t;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (activated)
        {
            t += Time.deltaTime;
            if (t >= detonationTimer) Explode();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((activationLayer & (1 << collision.gameObject.layer)) != 0 && collision.relativeVelocity.magnitude >= explosionThreshold)
        {
            Explode();
        }
    }

    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Damageable damageable))
            {
                damageable.RecieveDamage(10, (collider.transform.position - transform.position).normalized * explosionImpulse);
            }
        }

        Instantiate(particlePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Activate()
    {
        activated = true;
    }
}
