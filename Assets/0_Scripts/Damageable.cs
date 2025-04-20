using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] ParticleSystem onFireParticles;
    [SerializeField] Gradient tickColor;

    Renderer[] m_renderers;
    Material[] materials;

    [Header("Setup")]
    public bool destroyOnDeath;
    public bool burning;

    [Header("Settings")]
    public int health;
    public float knockbackForce = 1;
    public UnityEvent OnDeath;
    public UnityEvent OnPlayerVictim;

    private float t;
    private float tickT = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        m_renderers = GetComponentsInChildren<Renderer>();
        materials = new Material[m_renderers.Length];

        for (int i = 0; i < m_renderers.Length; i++)
        {
            materials[i] = m_renderers[i].material;
        }
    }

    private void Update()
    {
        ApplyBurningEffect();
    }

    public void RecieveDamage(int damage, Vector2 impactForce, bool playerInteraction)
    {
        health -= damage;
        rb.AddForce(impactForce * rb.mass * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(DamageTick(8));

        if (health <= 0)
        {
            if (playerInteraction) OnPlayerVictim.Invoke();

            OnDeath.Invoke();

            if (destroyOnDeath) 
                Destroy(gameObject);
        }
    }

    #region Fire
    public void BurningEffect(float duration)
    {
        if (!burning || t < duration)
        {
            t = duration;
        }
    }

    private void ApplyBurningEffect()
    {
        if (t > 0 && !burning) burning = true;

        if (burning)
        {
            tickT += Time.deltaTime;
            t -= Time.deltaTime;

            if (tickT > 0.5f)
            {
                RecieveDamage (1, Vector2.zero, true);
                tickT = 0;
            }

            if (!onFireParticles.isPlaying) onFireParticles.Play();
        }
        else
        {
            if (onFireParticles.isPlaying) onFireParticles.Stop();
        }

        if (t <= 0 && burning) burning = false;
    }

    #endregion

    IEnumerator DamageTick(float speed)
    {
        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime * speed;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetColor("_TickColor", tickColor.Evaluate(t) * 3f);
            }

            yield return null;
        }

        while (t >= 0)
        {
            t -= Time.deltaTime * speed;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetColor("_TickColor", tickColor.Evaluate(t));
            }

            yield return null;
        }

    }
}

