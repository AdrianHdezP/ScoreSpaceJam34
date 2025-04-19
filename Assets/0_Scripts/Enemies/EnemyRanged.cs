using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float detectionRange;
    [SerializeField] float aggroTimer;

    [Header("ATTACK")]
    [SerializeField] float fireTime;
    [SerializeField] Transform arrowSpawnPoint;
    [SerializeField] Arrow arrowPrefab;

    float fireT;

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
        distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
        directionToPlayer = (player.transform.position - transform.position).normalized;

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
    }

    void LookAtPlayer()
    {
        transform.up = directionToPlayer;
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
}
