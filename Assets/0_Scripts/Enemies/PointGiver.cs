using UnityEngine;

public class PointGiver : MonoBehaviour
{
    private PointManager pointsManager;

    private void Awake()
    {
        pointsManager = FindFirstObjectByType<PointManager>();
    }

    public void AddPoints(int pointsToAdd) => pointsManager.AddPoints(pointsToAdd);
}
