using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    PlayerController player;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();

    }

    private void Start()
    {
      //  agent.updateRotation = false;
      //  agent.updateUpAxis = false;
    }
    private void Update()
    {
        agent.transform.localPosition = Vector3.zero;
        agent.SetDestination(player.transform.position);
    }

    private void FixedUpdate()
    {
        if (player && Vector2.Distance(transform.position, player.transform.position) > agent.stoppingDistance)
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
