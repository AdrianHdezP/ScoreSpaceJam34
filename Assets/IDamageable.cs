using UnityEngine;

public interface IDamageable
{
    public static bool burning;

    public void RecieveDamage(int damage, Vector2 impactForce);
}
