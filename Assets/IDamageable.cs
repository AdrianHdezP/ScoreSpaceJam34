using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public bool burning;
    public bool destroyOnDeath;
    public int health;
    public UnityEvent OnDeath;
    float t;

    float tickT = 0;

    public void BurningEffect(float duration)
    {
        if (!burning || t < duration)
        {
            t = duration;
        }
    }
    void ApplyBurningEffect()
    {
        if (t > 0 && !burning) burning = true;

        if (burning)
        {
            tickT += Time.deltaTime;
            t -= Time.deltaTime;

            if (tickT > 0.5f)
            {
                RecieveDamage(1, Vector2.zero);
                tickT = 0;
            }
        }

        if (t <= 0 && burning) burning = false;
    }
    public void RecieveDamage(int damage, Vector2 impactForce)
    {
        health -= damage;
        //RecieveDamageVisual();

        if (health <= 0)
        {
            OnDeath.Invoke();
            if(destroyOnDeath) Destroy(gameObject);
        }
    }


    private void Update()
    {
        ApplyBurningEffect();
    }
}
