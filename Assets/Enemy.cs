using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] int health;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void RecieveDamage(int damage, Vector2 impactForce)
    {
        rb.AddForce(impactForce.normalized * rb.mass * 30, ForceMode2D.Impulse);
        Destroy(gameObject,2);
    }
}
