using System.Collections;
using TMPro;
using UnityEngine;

public class FinalPointsManager : MonoBehaviour
{
    private PointManager pointsManager;

    [SerializeField] private TextMeshProUGUI poinsTMP;
    [SerializeField] private TextMeshProUGUI timeTMP;

    private void Awake()
    {
        pointsManager = FindFirstObjectByType<PointManager>();
    }

    private void Start()
    {
        poinsTMP.text = pointsManager.points.ToString();
        timeTMP.text = Clock(pointsManager.totalTime);

        StartCoroutine(LerpPointsValue(2.5f ,0.5f));
    }

    private IEnumerator LerpPointsValue(float waitTime, float speed)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        int initialPoints = pointsManager.points;
        int finalPoints = pointsManager.points + pointsManager.timePoints;
        int currentPoints = initialPoints;

        float initialTime = pointsManager.totalTime;

        float t = 0;

        while (t < 1)
        {
            t += Time.unscaledDeltaTime * speed;

            currentPoints = (int)Mathf.Lerp(initialPoints, finalPoints, t);
            poinsTMP.text = currentPoints.ToString();

            timeTMP.text = Clock(Mathf.Lerp(initialTime, 0, t));

            yield return null;
        }
    }

    private string Clock(float timeElapsed)
    {
        int minutes = (int)(timeElapsed / 60f);
        int seconds = (int)(timeElapsed - minutes * 60f);
        return string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));
    }
}
