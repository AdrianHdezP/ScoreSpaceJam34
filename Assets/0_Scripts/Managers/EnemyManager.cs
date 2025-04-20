using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private PointManager pointManager;

    private List<Enemy> meleeEnemyList = new List<Enemy>();
    private List<EnemyRanged> rangedEnemyList = new List<EnemyRanged>();

    [Header("Setup")]
    [SerializeField] private int meleeEnemyNumber;
    [SerializeField] private GameObject meleeEnemyPrefab;
    [SerializeField] private float meleeSpawnInterval;
    [SerializeField] private Transform[] meleeSpawnPoints;
    [SerializeField] private int rangeEnemyNumber;
    [SerializeField] private GameObject rangeEnemyPrefab;
    [SerializeField] private float rangedSpawnInterval;
    [SerializeField] private Transform[] rangeSpawnPoints;
    [Space]
    public Transform[] meleeWayPoints;

    private float meleeT;
    private float rangedT;

    private void Awake()
    {
        pointManager = FindFirstObjectByType<PointManager>();
    }

    private void Start()
    {
        meleeT = meleeSpawnInterval;
        rangedT = rangedSpawnInterval;

        SpawnInitialEnemies();
    }

    private void SpawnInitialEnemies()
    {
        for (int i = 0; i < meleeEnemyNumber; i++)
            SpawnMelee();

        for (int i = 0;i < rangeEnemyNumber; i++)
            SpawnRanged();
    }

    private void Update()
    {
        Clock();
    }

    private void Clock()
    {
        if (pointManager.timeOut)
            return;

        if (meleeT > 0 && meleeEnemyList.Count < meleeEnemyNumber)
            meleeT -= Time.deltaTime;

        if (rangedT > 0 && rangedEnemyList.Count < rangeEnemyNumber)
            rangedT -= Time.deltaTime;

        if (meleeEnemyList.Count < meleeEnemyNumber && meleeT <= 0)
        {
            meleeT = meleeSpawnInterval;
            SpawnMelee();
        }

        if (rangedEnemyList.Count < rangeEnemyNumber && rangedT <= 0)
        {
            rangedT = rangedSpawnInterval;
            SpawnRanged();
        }
    }

    #region Spawn Methods

    private GameObject InstantiateEnemy(GameObject enemyPrefab, Transform spawnPoint)
    {
        Vector3 rangedPos = spawnPoint.position;
        rangedPos.x += Random.Range(-3,3);
        rangedPos.y += Random.Range(-3,3);

       return Instantiate(enemyPrefab, rangedPos, Quaternion.identity);
    }

    private void SpawnMelee() => InstantiateEnemy(meleeEnemyPrefab, ReturnRandomTransform(meleeSpawnPoints));

    private void SpawnRanged() => InstantiateEnemy(rangeEnemyPrefab, ReturnRandomTransform(rangeSpawnPoints));

    #endregion

    #region Add && Remove To List Mehods

    public void AddMelee(Enemy enemyToAdd)
    {
        if (!meleeEnemyList.Contains(enemyToAdd))
            meleeEnemyList.Add(enemyToAdd);
    }

    public void AddRange(EnemyRanged enemyToAdd)
    {
        if (!rangedEnemyList.Contains(enemyToAdd))
            rangedEnemyList.Add(enemyToAdd);
    }

    public void RemoveMelee(Enemy enemyToRemove) 
    {
        if (meleeEnemyList.Contains(enemyToRemove))
            meleeEnemyList.Remove(enemyToRemove);
    } 

    public void RemoveRanged(EnemyRanged enemyToRemove)
    {
        if (rangedEnemyList.Contains(enemyToRemove))
            rangedEnemyList.Remove(enemyToRemove);
    }

    #endregion

    #region Return Methods

    public List<Enemy> ReturnMeleeEnemyList() => meleeEnemyList;

    public List<EnemyRanged> ReturnEnemyRangedList() => rangedEnemyList;

    private Transform ReturnRandomTransform(Transform[] myTransforms) => myTransforms[Random.Range(0, myTransforms.Length)];

    #endregion

}
