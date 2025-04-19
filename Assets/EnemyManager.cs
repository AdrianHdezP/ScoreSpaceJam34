using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float detectionRange;
    public float aggroTimer;

    public Transform[] rangeSpawnPoints;
    public Transform[] meleeSpawnPoints;
    public Transform[] meleeWayPoints;
}
