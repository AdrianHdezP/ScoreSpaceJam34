using DG.Tweening;
using UnityEngine;

public class SectionHolder : MonoBehaviour
{
    public Transform[] sections;
    int? currentSection;

    [SerializeField] Transform menuHolder;
    [SerializeField] Vector3 centerPoint;
    [SerializeField] Vector3 activatePoint;
    [SerializeField] Vector3 deactivatePoint;

    private void Start()
    {
        Highscore data = MainSingletone.inst.score.GetStoredScore();

        if (data != null)
        {
            menuHolder.gameObject.SetActive(true);
            ActivateSection(3);
        }
        else
        {
            menuHolder.gameObject.SetActive(false);
            ActivateSection(0);
        }
    }

    public void ActivateSection(int index)
    {
        if (index == currentSection) return;

        foreach (var section in sections)
        {
            section.DOComplete();

            if (currentSection != null && section == sections[(int)currentSection])
            {
                section.DOLocalMove(deactivatePoint, 0.8f).SetEase(Ease.InBack).OnComplete(() => section.gameObject.SetActive(false));
            }
            else
            {
                section.gameObject.SetActive(false);
            }
        }

        if (index < sections.Length)
        {
            sections[index].localPosition = activatePoint;
            sections[index].gameObject.SetActive(true);
            sections[index].DOLocalMove(centerPoint, 2f).SetEase(Ease.OutBounce);
            currentSection = index;
        }
        else
        {
            MainSingletone.inst.sceneControl.FadeOut(2);
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


