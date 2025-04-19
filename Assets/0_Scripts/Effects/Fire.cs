
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float duration;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Damageable damageable))
        {
            damageable.BurningEffect(duration);
        }
    }
}
