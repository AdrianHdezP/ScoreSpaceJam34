using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed;

    [Header("ATTACK")]
    [SerializeField] int chargeDamage;
    [SerializeField] float chargeSpeed;
    [SerializeField] float chargeCooldown;
    [SerializeField] float chargeDistance;
    [SerializeField] float prechargeTime;
    [SerializeField] float chargeknockback;
    [SerializeField] LayerMask activationLayer;


    Vector2 chargeStartPos;
    float chargeCooldownT;
    float prechargeT;
    bool isCharging;
    bool isPrecharging;

    bool isAggro;
    float aggroT;

    float distanceToPlayer;
    Vector2 directionToPlayer;

    PlayerController player;
    EnemyManager manager;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();
        manager = FindFirstObjectByType<EnemyManager>();
    }

    private void Start()
    {
      //  agent.updateRotation = false;
      //  agent.updateUpAxis = false;
    }
    private void Update()
    {
        agent.transform.localPosition = Vector3.zero;
        distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
        directionToPlayer = (player.transform.position - transform.position).normalized;

        if (isAggro)
        {
            agent.SetDestination(player.transform.position);

            if (distanceToPlayer > manager.detectionRange)
            {
                aggroT -= Time.deltaTime;
            }
            else
            {
                aggroT = manager.aggroTimer;
            }
        }


        if (isAggro && aggroT < 0)
        {
            isAggro = false;
        }
        else if (!isAggro && aggroT > 0)
        {
            isAggro = true;
        }
    }
    private void FixedUpdate()
    {
        if (player && Vector2.Distance(transform.position, player.transform.position) > agent.stoppingDistance)
        {
            rb.AddForce(agent.desiredVelocity * rb.mass * moveSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((activationLayer & (1 << collision.gameObject.layer)) != 0 && isCharging)
        {
            if (collision.gameObject.TryGetComponent(out Damageable damageable))
            {
                damageable.RecieveDamage(chargeDamage, (collision.transform.position - transform.position).normalized * chargeknockback);
            }

            isCharging = false;
        }
    }


    void ChargeManager()
    {
        if (!isCharging)
        {
            if (!isPrecharging && distanceToPlayer < chargeDistance * 0.85f)
            {
                prechargeT = 0;
                isPrecharging = true;
            }

            if (isPrecharging)
            {
                if (prechargeT < prechargeTime) prechargeT += Time.deltaTime;
                else
                {
                    isPrecharging = false;
                    StartCharge();
                }
            }
        }
    }
    void StartCharge()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        chargeStartPos = transform.position;
        rb.AddForce(direction * chargeSpeed * rb.mass, ForceMode2D.Impulse);

        chargeCooldownT = chargeCooldown;
        isCharging = true;
    }

    void LookAtPlayer()
    {
        transform.rotation = Quaternion.LookRotation()
    }

    void LookAtDirection()
    {

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
