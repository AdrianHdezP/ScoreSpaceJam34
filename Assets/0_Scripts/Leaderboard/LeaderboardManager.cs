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
    [SerializeField] int entriesPerPag;
    int currentPage;
    int lastPage;
    Entry[] entries;


    [SerializeField] Button refreshButton;

    [SerializeField] Button nextPageButton;
    [SerializeField] Button prevPageButton;
    [SerializeField] TextMeshProUGUI currentPageText;

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
    private void Update()
    {
        SetPageScroller();
    }

    #region LEADERBOARD
    public void GetLeaderboard()
    {
        refreshButton.interactable = false;
        Leaderboards.MyLeaderboard.GetEntries(onEntriesLoaded, onEntriesError);
    }
    void onEntriesLoaded(Entry[] entries_)
    {
        entries = entries_;
        currentPage = 0;
        lastPage = Mathf.FloorToInt(entries.Length / entriesPerPag);

        LoadLeaderboardPage();
    }
    void onEntriesError(string error)
    {
        Debug.LogError(error);

        errorTextMesh.gameObject.SetActive(true);
        refreshButton.interactable = true;
    }

    void LoadLeaderboardPage()
    {
        int loopLength = entries.Length;

        while (nameTexts.Count > 0)
        {
            GameObject inst = nameTexts[0].gameObject;
            nameTexts.RemoveAt(0);
            Destroy(inst);
        }

        nameTexts = new List<TextMeshProUGUI>();
        int elementsLoaded = 0;

        for (int i = currentPage * entriesPerPag; i < loopLength && elementsLoaded < entriesPerPag; i++)
        {
            //Debug.Log("SETTING NAME: " + entries[i].Username + " & SCORE: " + entries[i].Score.ToString("0000"));
            //entry.Rank para coger el ranking

            TextMeshProUGUI text = Instantiate(textPrefab, holder);
            nameTexts.Add(text);

            Debug.Log(loopLength);

            if (!entries[i].IsMine()) nameTexts[elementsLoaded].text = "(" + entries[i].Rank + ") " + entries[i].Username + "-" + entries[i].Score.ToString("0000") + " Pts";
            else
            {
                nameTexts[elementsLoaded].text = "(" + entries[i].Rank + ") " + entries[i].Username + "-" + entries[i].Score.ToString("0000") + " Pts";
                nameTexts[elementsLoaded].color = ownedColor;
            }

            elementsLoaded++;
        }

        errorTextMesh.gameObject.SetActive(false);
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

    public void LoadNextPage()
    {
        currentPage++;
        LoadLeaderboardPage();
    }
    public void LoadPrevPage()
    {
        currentPage--;
        LoadLeaderboardPage();
    }

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
    void SetPageScroller()
    {
        currentPageText.text = currentPage.ToString() + "/" + lastPage.ToString();

        if (currentPage >= lastPage && entries != null && entries.Length > 0) nextPageButton.interactable = false;
        else nextPageButton.interactable = true;

        if (currentPage <= 0 && entries != null && entries.Length > 0) prevPageButton.interactable = false;
        else prevPageButton.interactable = true;
    }
}
