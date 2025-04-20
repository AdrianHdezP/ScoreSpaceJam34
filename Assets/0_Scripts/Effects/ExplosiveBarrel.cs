using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExplosiveBarrel : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private GameObject barrelVisuals;
    [SerializeField] float detonationTimer;
    [SerializeField] float explosionRange;
    [SerializeField] float explosionImpulse;
    [SerializeField] float impulseMultiplier;
    [SerializeField] float explosionThreshold = 5;
    [SerializeField] LayerMask activationLayer;

    [SerializeField] ParticleSystem particlePrefab;

    bool activated;
    bool death;
    float t;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
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
                damageable.RecieveDamage(10, (collider.transform.position - transform.position).normalized * explosionImpulse, true);
            }
        }

        if (!death)
            StartCoroutine(DeathSecuence());
    }

    private IEnumerator DeathSecuence()
    {
        death = true;
        audioSource.Play();
        barrelVisuals.SetActive(false);
        Instantiate(particlePrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2.5f);

        Destroy(gameObject);
    }

    public void Activate()
    {
        activated = true;
    }
}
