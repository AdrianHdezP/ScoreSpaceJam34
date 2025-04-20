using TMPro;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    private PlayerController playerController;
    private EnemyManager enemyManager;

    [Header("Setup")]
    [SerializeField] private TextMeshProUGUI pointTMP;
    [SerializeField] private TextMeshProUGUI timeTMP;
    [SerializeField] private GameObject timeOutPanel;

    [Header("Settings")]
    [SerializeField] private float startTime = 30;
    [SerializeField] private float timeToAdd = 15;
    private int minutes;
    private int seconds;
    private float timeElapsed;

    public int points { get; private set; } = 0;
    public int timePoints { get; private set; }
    public float totalTime { get; private set; }
    public bool timeOut { get; private set; }

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        enemyManager = FindFirstObjectByType<EnemyManager>();
    }

    private void Start()
    {
        timeElapsed = startTime;
    }

    private void Update()
    {
        totalTime += Time.deltaTime;

        UpdatePoints();
        Clock();
        TimeOut();
    }

    private void UpdatePoints() => pointTMP.text = points.ToString();

    public void AddPoints(int pointsToAdd) => points += pointsToAdd;

    private void Clock()
    {
        timeElapsed -= Time.deltaTime;
        minutes = (int)(timeElapsed / 60f);
        seconds = (int)(timeElapsed - minutes * 60f);
        timeTMP.text = string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));
    }

    public void AddTime() => timeElapsed += timeToAdd;

    private void TimeOut()
    {
        if (timeElapsed <= 0 && !timeOut)
        {
            timeOut = true;

            timePoints = Mathf.FloorToInt(totalTime * 10);
            MainSingletone.inst.score.SetScore(points + timePoints);

            MainSingletone.inst.sceneControl.gM.FreezeGame();
            MainSingletone.inst.sceneControl.gM.ended = true;

            timeOutPanel.SetActive(true);
            //MainSingletone.inst.sceneControl.FadeOut(1);
        }
    }
}
