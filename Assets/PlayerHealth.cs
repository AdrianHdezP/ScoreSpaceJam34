using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour , IDamageable
{
    [SerializeField] int health = 100;

    public void RecieveDamage(int damage, Vector2 impactForce)
    {
        health -= damage;
        //RecieveDamageVisual();

        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }


  //  void RecieveDamageVisual()
  //  {
  //      StartCoroutine(LerpColor(Color.red, 20, 0.1f));
  //  }
  //  IEnumerator LerpColor(Color hitColor, float lerpSpeed, float hitDuration)
  // {
  //     float t = 0;
  //
  //     while (t <= 1)
  //     {
  //         t += Time.deltaTime * lerpSpeed;
  //
  //         for (int i = 0; i < renderers.Length; i++)
  //         {
  //             renderers[i].color = Color.Lerp(startColors[i], hitColor, t);
  //         }
  //
  //         yield return null;
  //     }
  //
  //     t = 0;
  //     while (t < hitDuration)
  //     {
  //         t += Time.deltaTime;
  //         yield return null;
  //     }
  //
  //     t = 0;
  //     while (t <= 1)
  //     {
  //         t += Time.deltaTime * lerpSpeed;
  //
  //         for (int i = 0; i < renderers.Length; i++)
  //         {
  //             renderers[i].color = Color.Lerp(hitColor, startColors[i], t);
  //         }
  //
  //         yield return null;
  //     }
  // }
}
