using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float detectionRange;
    [SerializeField] float aggroTimer;
    [SerializeField] float moveSpeed;

    [Header("ATTACK")]
    [SerializeField] float stoppingDistance;
    [SerializeField] float fireTime;
    [SerializeField] Transform arrowSpawnPoint;
    [SerializeField] Arrow arrowPrefab;

    [HideInInspector] public bool frezze;

    float fireT;

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

        manager.AddRange(this);
    }
    private void OnDestroy()
    {
        manager.RemoveRanged(this);
    }

    private void Start()
    {
        //  agent.updateRotation = false;
        //  agent.updateUpAxis = false;
    }
    private void Update()
    {
        if (frezze)
        {
            agent.isStopped = true;
            return;
        }

        agent.transform.localPosition = Vector3.zero;
        distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
        directionToPlayer = (player.transform.position - transform.position).normalized;
        moveDirection = rb.linearVelocity.normalized;

        if (isAggro)
        {
            if (distanceToPlayer > detectionRange)
            {
                aggroT -= Time.deltaTime;
            }
            else
            {
                aggroT = aggroTimer;
            }

            ControlWeapon();
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

        if (isAggro) LookAtPlayer();
        else LookAtDirection();
    }

    void ControlWeapon()
    {
        if (fireT > fireTime)
        {
            Shoot();
            fireT = 0;
        }
        else
        {
            fireT += Time.deltaTime;
        }
    }

    void Shoot()
    {
        Arrow arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        arrow.shooter = this;
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
        if (!wayPoint || Vector2.Distance(transform.position, wayPoint.position) < stoppingDistance + 2f) wayPoint = manager.meleeWayPoints[Random.Range(0, manager.meleeWayPoints.Length)];

        if (wayPoint)
        {
            agent.SetDestination(wayPoint.position);
        }
    }
}
