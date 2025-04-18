using Dan.Main;
using Dan.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Username : MonoBehaviour
{
    [SerializeField] TMP_InputField inputName;

    [SerializeField] Button usernameButton;
    [SerializeField] Button continueButton;

    [SerializeField] TextMeshProUGUI errorText;

    private void OnEnable()
    {
        if (MainSingletone.inst.score)
        {
            Highscore data = MainSingletone.inst.score.GetStoredScore();
            if (data != null) inputName.text = data.username;
        }

        inputName.interactable = true;

        usernameButton.interactable = true;
        continueButton.interactable = true;
    }
    private void Update()
    {
        if (inputName.text == MainSingletone.inst.score.username)
        {
            continueButton.gameObject.SetActive(true);
            usernameButton.gameObject.SetActive(false);
        }
        else
        {
            continueButton.gameObject.SetActive(false);
            usernameButton.gameObject.SetActive(true);
        }
    }

    //SET USERNAME
    public void TrySetUsername()
    {
        Leaderboards.MyLeaderboard.GetEntries(SetName, OnErrorEntries);
    }
    void SetName(Entry[] uploadedEntries)
    {
        string username = inputName.text;
        bool exists = false;

        foreach (Entry entry in uploadedEntries)
        {
            if (entry.Username == username && !entry.IsMine())
            {
                exists = true;
                break;
            }
        }

        if (exists)
        {
            if (MainSingletone.inst.language.LanguageId == 1) errorText.text = "USERNAME ALREADY EXISTS, PLEASE SELECT ANOTHER ONE";
            if (MainSingletone.inst.language.LanguageId == 2) errorText.text = "USUARIO YA EXISTE, POR FAVOR ELIGE OTRO USUARIO";

            errorText.gameObject.SetActive(true);
            usernameButton.interactable = true;

        }
        else if (username == "")
        {
            if (MainSingletone.inst.language.LanguageId == 1) errorText.text = "USERNAME CAN NOT BE EMPTY, STOP BEING EDGY";
            if (MainSingletone.inst.language.LanguageId == 2) errorText.text = "NOMBRE NO PUEDE ESTAR VACIO, DEJA DE SER TAN EDGY";

            errorText.gameObject.SetActive(true);
            usernameButton.interactable = true;
        }
        else if (username.ToLower().Contains("name"))
        {
            if (MainSingletone.inst.language.LanguageId == 1) errorText.text = "STOP TRYING TO BE FUNNY THIS IS A SERIOUS MATTER!";
            if (MainSingletone.inst.language.LanguageId == 2) errorText.text = "ESTO TE PARECE UNA BROMA?! ES COSA SERIA";

            errorText.gameObject.SetActive(true);
            usernameButton.interactable = true;
        }
        else
        {
            errorText.gameObject.SetActive(false);

            MainSingletone.inst.score.SetLeaderBoardEntry(username, 0);
            MainSingletone.inst.score.SetUsername(username);
            MainSingletone.inst.score.TryStoreScore();

            inputName.interactable = false;
            usernameButton.interactable = false;
            if (FindFirstObjectByType<SectionHolder>()) FindFirstObjectByType<SectionHolder>().GoToNextSection();
        }
    }

    //ERROR LEADERBOARD
    void OnErrorEntries(string error)
    {
        Debug.LogError("FAILED: " + error);
    }
    void LockUsernameChange()
    {
        inputName.interactable = false;
        inputName.text = MainSingletone.inst.score.username;

        usernameButton.interactable = false;
        usernameButton.gameObject.SetActive(false);

        continueButton.gameObject.SetActive(true);
    }
}
