using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] float detectionRange;
    [SerializeField] float aggroTimer;
    [SerializeField] float moveSpeed;

    [Header("ATTACK")]
    [SerializeField] float stoppingDistance;
    [SerializeField] float fireTime;
    [SerializeField] Transform arrowSpawnPoint;
    [SerializeField] Arrow arrowPrefab;

    float fireT;

    bool isAggro;
    float aggroT;

    bool isDead;

    float distanceToPlayer;
    Vector2 directionToPlayer;
    Vector2 moveDirection;

    PlayerController player;
    EnemyManager manager;
    Transform wayPoint;
    AudioManager audioManager;
    AudioSource audioSource;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();
        manager = FindFirstObjectByType<EnemyManager>();
        audioManager = FindFirstObjectByType<AudioManager>();
        audioSource = GetComponent<AudioSource>();

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

    private void FixedUpdate()
    {
        if (MainSingletone.inst.sceneControl.gM.paused)
        {
            //agent.isStopped = true;
            return;
        }

        if (player && distanceToPlayer > stoppingDistance)
        {
            rb.AddForce(agent.desiredVelocity * rb.mass * moveSpeed);
        }
    }

    private void Update()
    {
        ControlVisuals();

        if (MainSingletone.inst.sceneControl.gM.paused)
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
            agent.SetDestination(player.transform.position);

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
            audioManager.PlayOneShoot(audioSource, audioManager.ReturnRandomAudio(audioManager.aldeano), 0.75f);
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
        transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);
    }
    void LookAtDirection()
    {
        if (moveDirection != Vector2.zero) transform.up = moveDirection;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
    }

    void MoveToWayPoint()
    {
        if (!wayPoint || Vector2.Distance(transform.position, wayPoint.position) < stoppingDistance + 2f) wayPoint = manager.meleeWayPoints[Random.Range(0, manager.meleeWayPoints.Length)];

        if (wayPoint)
        {
            agent.SetDestination(wayPoint.position);
        }
    }

    void ControlVisuals()
    {
        if (rb.linearVelocity.magnitude < 0.5f) anim.SetBool("Idle", true);
        else anim.SetBool("Idle", false);

        if (rb.linearVelocity.magnitude >= 0.5f) anim.SetBool("Move", true);
        else anim.SetBool("Move", false);

        float speed = Mathf.Clamp(Vector2.Dot(transform.up, rb.linearVelocity), 0, 15f);
        anim.SetFloat("Speed", speed * 0.5f);
    }

    public void TriggerDeath()
    {
        if (!isDead)
        {
            isDead = true;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
