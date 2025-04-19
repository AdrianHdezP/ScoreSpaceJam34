using TMPro;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private TextMeshProUGUI pointTMP;
    [SerializeField] private TextMeshProUGUI timeTMP;

    [Header("Settings")]
    [SerializeField] private float startTime = 30;
    [SerializeField] private float timeToAdd = 15;
    private int points = 0;
    private int minutes;
    private int seconds;
    private float timeElapsed;
    private bool timeOut = false;

    private void Start()
    {
        timeElapsed = startTime;
    }

    private void Update()
    {
        UpdatePoints();
        Clock();
        //TimeOut();
    }

    private void UpdatePoints() => pointTMP.text = points.ToString();

    public void AddPoints(int pointsToAdd) => points += pointsToAdd;

    private void Clock()
    {
        timeElapsed -= Time.deltaTime;
        minutes = (int)(timeElapsed / 60f);
        seconds = (int)(timeElapsed - minutes * 60f);
        timeTMP.text = string.Format("{0}:{1}", minutes, seconds);
    }

    public void AddTime() => timeElapsed += timeToAdd;

    private void TimeOut()
    {
        if (timeElapsed <= 0 && !timeOut)
        {
            timeOut = true;
            MainSingletone.inst.sceneControl.FadeOut(1);
        }
    }
}
