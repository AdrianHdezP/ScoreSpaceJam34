using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour , IDamageable
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] int health;
    [SerializeField] float moveSpeed;

    PlayerHealth playerHealth;

    public void RecieveDamage(int damage, Vector2 impactForce)
    {
        health -= damage;

       // RecieveDamageVisual();
        rb.AddForce(impactForce, ForceMode2D.Impulse);

        if (health <= 0) Destroy(gameObject);
    }

    private void Awake()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();

    }

    private void Start()
    {
      //  agent.updateRotation = false;
      //  agent.updateUpAxis = false;
    }
    private void Update()
    {
        agent.transform.localPosition = Vector3.zero;
        agent.SetDestination(playerHealth.transform.position);

        if (playerHealth && Vector2.Distance(transform.position, playerHealth.transform.position) > agent.stoppingDistance)
        {
            rb.AddForce(agent.desiredVelocity * rb.mass * moveSpeed);
        }
    }


  //  void RecieveDamageVisual()
  //  {
  //      StartCoroutine(LerpColor(Color.red, 20, 0.1f));
  //  }
  //  IEnumerator LerpColor(Color hitColor, float lerpSpeed, float hitDuration)
  //  {
  //      float t = 0;
  //
  //      while (t <= 1)
  //      {
  //          t += Time.deltaTime * lerpSpeed;
  //
  //          for (int i = 0; i < renderers.Length; i++)
  //          {
  //              renderers[i].color = Color.Lerp(startColors[i], hitColor, t);
  //          }
  //
  //          yield return null;
  //      }
  //
  //      t = 0;
  //      while (t < hitDuration)
  //      {
  //          t += Time.deltaTime;
  //          yield return null;
  //      }
  //
  //      t = 0;
  //      while (t <= 1)
  //      {
  //          t += Time.deltaTime * lerpSpeed;
  //
  //          for (int i = 0; i < renderers.Length; i++)
  //          {
  //              renderers[i].color = Color.Lerp(hitColor, startColors[i], t);
  //          }
  //
  //          yield return null;
  //      }
  //  }
}
