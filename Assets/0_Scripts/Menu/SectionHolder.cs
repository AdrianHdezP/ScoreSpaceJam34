using DG.Tweening;
using UnityEngine;

public class SectionHolder : MonoBehaviour
{
    public Tab[] sections;
    int? currentSection;

    [SerializeField] Transform menuHolder;
    [SerializeField] Vector3 centerPoint;
    [SerializeField] Vector3 activatePoint;
    [SerializeField] Vector3 deactivatePoint;

   // private void Start()
   // {
   //     Highscore data = MainSingletone.inst.score.GetStoredScore();
   //
   //     if (data != null)
   //     {
   //        // menuHolder.gameObject.SetActive(true);
   //        // ActivateSection(2);
   //     }
   //     else
   //     {
   //        // menuHolder.gameObject.SetActive(false);
   //         ActivateSection(2);
   //     }
   // }

    public void ActivateSection(int index)
    {
        foreach (var section in sections)
        {
            if (section != null)
            {
                if (section == sections[index]) section.ToogleMaximize();
                //if (section != sections[index]) section.Minimize();

                currentSection = index;
            }
        }
    } 
    public void GoToNextSection()
    {
        PlayerPrefs.Save();
        ActivateSection((int)currentSection + 1);
    }

    public void GoToGame()
    {
        PlayerPrefs.Save();
        MainSingletone.inst.sceneControl.FadeOut(2);
    }

    public void OpenURl()
    {
        Application.OpenURL("https://games-for-robots.itch.io/");
    }
    public void OpenURlAdri()
    {
        Application.OpenURL(" https://adrianhdez.itch.io/");
    }
}


