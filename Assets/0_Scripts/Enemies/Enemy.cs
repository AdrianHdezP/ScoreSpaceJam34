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
    [SerializeField] float detectionRange;
    [SerializeField] float aggroTimer;

    [Header("ATTACK")]
    [SerializeField] int chargeDamage;
    [SerializeField] float chargeSpeed;
    [SerializeField] float chargeCooldown;
    [SerializeField] float chargeDistance;
    [SerializeField] float prechargeTime;
    [SerializeField] float chargeknockback;
    [SerializeField] float stoppingDistance;
    [SerializeField] LayerMask activationLayer;

    Vector2 chargeStartPos;
    float chargeCooldownT;
    float prechargeT;
    bool isCharging;
    float chargingT;

    bool isPrecharging;

    bool isAggro;
    float aggroT;

    float distanceToPlayer;
    Vector2 directionToPlayer;
    Vector2 moveDirection;

    PlayerController player;
    EnemyManager manager;
    Transform wayPoint;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();
        manager = FindFirstObjectByType<EnemyManager>();

        manager.AddMelee(this);
    }
    private void OnDestroy()
    {
        manager.RemoveMelee(this);
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
        moveDirection = rb.linearVelocity.normalized;

        if (isAggro)
        {
            agent.SetDestination(player.transform.position);

            if (distanceToPlayer > detectionRange)
            {
                aggroT -= Time.deltaTime;
            }
            else
            {
                aggroT = aggroTimer;
            }

            ChargeManager();
        }
        else
        {
            MoveToWayPoint();
        }

        if (!isAggro && distanceToPlayer < detectionRange)
        {
            isAggro = true;
            aggroT = aggroTimer;
        }

        if (isAggro && aggroT < 0)
        {
            isAggro = false;
        }
        else if (!isAggro && aggroT > 0)
        {
            isAggro = true;
        }

        if (isCharging || isPrecharging) LookAtPlayer();
        else LookAtDirection();
    }
    private void FixedUpdate()
    {
        if (player && distanceToPlayer > stoppingDistance && !isCharging && !isPrecharging)
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
                damageable.RecieveDamage(chargeDamage, (collision.transform.position - transform.position).normalized * chargeknockback, false);
            }

            isCharging = false;
        }
    }
 

    void ChargeManager()
    {
        if (!isCharging && chargeCooldownT <= 0)
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
        else if (isCharging)
        {
            if (Vector2.Distance(chargeStartPos, transform.position) > chargeDistance)
            {
                isCharging = false;
            }

            if (chargingT < 2) chargingT += Time.deltaTime;
            else isCharging = false;
        }

        if (chargeCooldownT > 0 && !isCharging) chargeCooldownT -= Time.deltaTime;
    }
    void StartCharge()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        chargeStartPos = transform.position;
        rb.AddForce(direction * chargeSpeed * rb.mass, ForceMode2D.Impulse);

        chargeCooldownT = chargeCooldown;
        isCharging = true;
        isPrecharging = false;
        chargingT = 0;
    }

    void LookAtPlayer()
    {
        transform.up = directionToPlayer;
    }
    void LookAtDirection()
    {
        if (moveDirection != Vector2.zero) transform.up = moveDirection;
    }

    void MoveToWayPoint()
    {
        if (!wayPoint || Vector2.Distance(transform.position, wayPoint.position) < stoppingDistance + 2f)  wayPoint = manager.meleeWayPoints[Random.Range(0, manager.meleeWayPoints.Length)];

        if (wayPoint)
        {
            agent.SetDestination(wayPoint.position);
        }
    }
   
}
