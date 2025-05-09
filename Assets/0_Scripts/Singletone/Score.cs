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
    public void DisplayError(string errorLog)
    {
        errortextHolder.gameObject.SetActive(true);
        errortextHolder.color = Color.white;
        errortextHolder.DOFade(0, 20).OnComplete(() => errortextHolder.gameObject.SetActive(false));

        errorText.text = errorLog;
        errorText.color = Color.red;
        errorText.DOFade(0, 20);
    }
}
