using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> meleeEnemyList;
    private List<EnemyRanged> rangedEnemyList;

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

    private void Start()
    {
        meleeT = meleeSpawnInterval;
        rangedT = rangedSpawnInterval;

        SpawnInitialEnemies();
    }

    private void SpawnInitialEnemies()
    {
        meleeEnemyList = new List<Enemy>();
        rangedEnemyList = new List<EnemyRanged>();

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

    private GameObject InstantiateEnemy(GameObject enemyPrefab, Transform spawnPoint) => Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

    private void SpawnMelee() => InstantiateEnemy(meleeEnemyPrefab, ReturnRandomTransform(meleeSpawnPoints));

    private void SpawnRanged() => InstantiateEnemy(rangeEnemyPrefab, ReturnRandomTransform(rangeSpawnPoints));

    #endregion

    #region Add && Remove To List Mehods

    public void AddMelee(Enemy enemyToAdd) => meleeEnemyList.Add(enemyToAdd);

    public void AddRange(EnemyRanged enemyToAdd) => rangedEnemyList.Add(enemyToAdd);

    public void RemoveMelee(Enemy enemyToRemove) => meleeEnemyList.Remove(enemyToRemove);

    public void RemoveRanged(EnemyRanged enemyToRemove) => rangedEnemyList.Remove(enemyToRemove);

    #endregion

    #region Return Methods

    public List<Enemy> ReturnEnemyList() => meleeEnemyList;

    public List<EnemyRanged> ReturnEnemyRangedList() => rangedEnemyList;

    private Transform ReturnRandomTransform(Transform[] myTransforms) => myTransforms[Random.Range(0, myTransforms.Length)];

    #endregion

}
