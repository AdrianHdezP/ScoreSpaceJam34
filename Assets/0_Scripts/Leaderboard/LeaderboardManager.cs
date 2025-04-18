using Dan.Main;
using Dan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    //LEADERBOARD
    [SerializeField] TextMeshProUGUI textPrefab;
    [SerializeField] Color ownedColor;
    [SerializeField] Transform holder;

    [SerializeField] Button refreshButton;

    List<TextMeshProUGUI> nameTexts = new();


    //LEADERBOARD
    [SerializeField] TextMeshProUGUI usernameMesh;
    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] TextMeshProUGUI errorTextMesh;

    //DEBUGGING
    //[SerializeField] TMP_InputField inputScore;

    private void OnEnable()
    {
        TryStoreScore();
        SetUserStuff();
    }
    private void Start()
    {
        TryStoreScore();
        //GetLeaderboard();
    }

    #region LEADERBOARD
    public void GetLeaderboard()
    {
        refreshButton.interactable = false;
        Leaderboards.MyLeaderboard.GetEntries(onEntriesLoaded, onEntriesError);
    }
    void onEntriesLoaded(Entry[] entries)
    {
        int loopLength = entries.Length;

        while (nameTexts.Count > 0)
        {
            GameObject inst = nameTexts[0].gameObject;
            nameTexts.RemoveAt(0);
            Destroy(inst);
        }

        nameTexts = new List<TextMeshProUGUI>();

        for (int i = 0; i < loopLength; i++)
        {
            //Debug.Log("SETTING NAME: " + entries[i].Username + " & SCORE: " + entries[i].Score.ToString("0000"));
            //entry.Rank para coger el ranking

            TextMeshProUGUI text = Instantiate(textPrefab, holder);
            nameTexts.Add(text);

            if (!entries[i].IsMine()) nameTexts[i].text = "(" + entries[i].Rank + ") " + entries[i].Username + "-" + entries[i].Score.ToString("0000") + " Pts";
            else
            {
                nameTexts[i].text = "(" + entries[i].Rank + ") " + entries[i].Username + "-" + entries[i].Score.ToString("0000") + " Pts";
                nameTexts[i].color = ownedColor;
            }
        }

        errorTextMesh.gameObject.SetActive(false);
        refreshButton.interactable = true;
    }
    void onEntriesError(string error)
    {
        Debug.LogError(error);

        errorTextMesh.gameObject.SetActive(true);
        refreshButton.interactable = true;
    }
    public void SetLeaderBoardEntry(string username, int score)
    {
        Leaderboards.MyLeaderboard.UploadNewEntry(username, score, (msg) =>
        {
            //Leaderboards.MyLeaderboard.ResetPlayer();
            GetLeaderboard();
        });
    }
    #endregion

    #region LOCAL STORAGING 
    public void TryStoreScore()
    {
        MainSingletone.inst.score.TryStoreScore();
    }
    #endregion

    //LOCK WHEN HAS USERNAME
    public void SetUserStuff()
    {
        Highscore data = MainSingletone.inst.score.GetStoredScore();

        if (data != null && MainSingletone.inst.score)
        {
            if (MainSingletone.inst.language.LanguageId == 1) usernameMesh.text = "Welcome back <br>" + MainSingletone.inst.score.username + "!";
            if (MainSingletone.inst.language.LanguageId == 2) usernameMesh.text = "Que bueno volver a verte <br>" + MainSingletone.inst.score.username + "!";

            bestScoreText.gameObject.SetActive(true);

            if (MainSingletone.inst.language.LanguageId == 1) bestScoreText.text = "Your Best " + data.score + " Pts";
            if (MainSingletone.inst.language.LanguageId == 2) bestScoreText.text = "Tu record " + data.score + " Pts";

            //LockUsernameChange();
        }
        else
        {
            if (MainSingletone.inst.language.LanguageId == 1) usernameMesh.text = "Welcome New Friend";
            if (MainSingletone.inst.language.LanguageId == 2) usernameMesh.text = "Bienvenido Amigo";

            bestScoreText.gameObject.SetActive(false);
        }
    }
}
