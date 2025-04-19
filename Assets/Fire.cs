
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] float tickTime;

    List<IDamageable> damageables = new List<IDamageable>();
    List<float> ts = new List<float>();

    private void Update()
    {
        for (int i = 0; i < damageables.Count; i++)
        {
            ts[i] += Time.deltaTime;

            if (ts[i] > tickTime)
            {
                ts[i] = 0;
                damageables[i].RecieveDamage(1, Vector2.zero);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            if (!damageables.Contains(damageable))
            {
                damageables.Add(damageable);
                ts.Add(0);

                damageable.RecieveDamage(1, Vector2.zero);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            if (damageables.Contains(damageable))
            {
                ts.RemoveAt(damageables.IndexOf(damageable));
                damageables.Remove(damageable);
            }
        }
    }
}
