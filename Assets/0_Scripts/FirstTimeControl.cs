using UnityEngine;

public class FirstTimeControl : MonoBehaviour
{
    [SerializeField] GameObject logo;
    [SerializeField] GameObject audioSetting;


    void Start()
    {
        //  Highscore data = MainSingletone.inst.score.GetStoredScore();
        //
        //  if (data != null)
        //  {
        //      ActivateLogoAnim();
        //  }
        //  else
        //  {
        //      ActivateAudioSlider();
        //  }

        ActivateLogoAnim();
    }

    public void ActivateLogoAnim()
    {
        logo.SetActive(true);
      //audioSetting.SetActive(false);
    }
    public void ActivateAudioSlider()
    {
        logo.SetActive(false);
       //audioSetting.SetActive(true);
    }

}
