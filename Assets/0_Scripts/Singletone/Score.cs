using Dan.Main;
using Dan.Models;
using DG.Tweening;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] Image errortextHolder;
    [SerializeField] TextMeshProUGUI errorText;
    public string username { get; private set; } = "(*** USERNAME ERROR ***)";
    public int score { get; private set; } = 0;

    private void Awake()
    {
        Highscore data = GetStoredScore();

        if (data != null) username = data.username;
        else username = "";
    }

    #region LOCAL STORAGING 
    public void TryStoreScore()
    {
        StoreScoreLocally();
        Leaderboards.MyLeaderboard.GetPersonalEntry(OnGetPersonalEntry, OnErrorGetPersonalEntry);
    }
    void OnGetPersonalEntry(Entry entry)
    {
        Highscore prevData = GetStoredScore();

        string path = Application.persistentDataPath + "/score.data";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        Highscore data = new Highscore(score, username);

        if (prevData != null && prevData.score > score) data.score = prevData.score;
        if (entry.Score > data.score) data.score = entry.Score;

        //Debug.Log("ENTRY SCORE: " + entry.Score);

        formatter.Serialize(stream, data);
        stream.Close();

        SetLeaderBoardEntry(username, data.score);

        LeaderboardManager leaderboard = FindFirstObjectByType<LeaderboardManager>();
        if (leaderboard != null)
        {
            leaderboard.SetUserStuff();
        }
    }
    void OnErrorGetPersonalEntry(string text)
    {
        Debug.LogError("FAILED: " + text);
        DisplayError();
    }
    public Highscore GetStoredScore()
    {
        string path = Application.persistentDataPath + "/score.data";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Highscore data = formatter.Deserialize(stream) as Highscore;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
    public void StoreScoreLocally()
    {
        Highscore prevData = GetStoredScore();

        string path = Application.persistentDataPath + "/score.data";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        Highscore data = new Highscore(score, username);

        if (prevData != null && prevData.score > score) data.score = prevData.score;

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("(INFO) Score stored locally: " + data.score);
    }
    #endregion

    #region LEADERBOARD
    public void SetLeaderBoardEntry(string username, int score)
    {
        Leaderboards.MyLeaderboard.UploadNewEntry(username, score);
    }
    #endregion

    #region GAME RELATED
    public void SetScore(int newScore)
    {
        score = newScore;
    }
    public void ConvertStringToScore(string s)
    {
        if (!int.TryParse(s, out int score)) score = 0;
        this.score = score;
    }
    public void SetUsername(string username)
    {
        this.username = username;
    }
    #endregion

    //DEBUGGING
    [ContextMenu("DELETE DATA")]
    void DeleteData()
    {
        string path = Application.persistentDataPath + "/score.data";

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        PlayerPrefs.DeleteAll();
    }

    void DisplayError()
    {
        errortextHolder.gameObject.SetActive(true);
        errortextHolder.color = Color.white;
        errortextHolder.DOFade(0, 20).OnComplete(() => errortextHolder.gameObject.SetActive(false));

        errorText.color = Color.red;
        errorText.DOFade(0, 20);
    }
}
