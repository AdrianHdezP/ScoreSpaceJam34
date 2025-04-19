using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Setup")]
    public bool destroyOnDeath;
    public bool burning;

    [Header("Settings")]
    public int health;
    public float knockbackForce = 1;
    public UnityEvent OnDeath;

    private float t;
    private float tickT = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ApplyBurningEffect();
    }

    public void RecieveDamage(int damage, Vector2 impactForce, bool playerInteraction)
    {
        health -= damage;
        rb.AddForce(impactForce * rb.mass * knockbackForce, ForceMode2D.Impulse);

        if (health <= 0)
        {
            if (playerInteraction)
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
                RecieveDamage(1, Vector2.zero, true);
                tickT = 0;
            }
        }

        if (t <= 0 && burning) burning = false;
    }

    #endregion

}
