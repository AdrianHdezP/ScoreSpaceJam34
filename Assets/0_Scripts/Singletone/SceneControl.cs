using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] Animator TVstatic;
    [SerializeField] Animator TVOff;

    public GameMaster gM;
    public int selectedScene;

    public enum Transitions
    {
        none,
        fade,
        tvStatic,
        tvOff
    }
    public Transitions current;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (current == Transitions.fade) FadeIn();
        else fadeImage.gameObject.SetActive(false);

        if (current == Transitions.tvStatic) TVStaticIn();
        else TVstatic.gameObject.SetActive(false);

        if (current == Transitions.tvOff) TVOffIn();
        else TVOff.gameObject.SetActive(false);

        gM = FindFirstObjectByType<GameMaster>();
    }

    void FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
        fadeImage.DOFade(0, 2).SetEase(Ease.InOutQuad).OnComplete(() => fadeImage.gameObject.SetActive(false));
    }   
    public void FadeOut(int sceneIndex)
    {
        Time.timeScale = 1;
        current = Transitions.fade;

        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        fadeImage.DOFade(1, 2).SetEase(Ease.InOutQuad).OnComplete(()=>SceneManager.LoadScene(sceneIndex));
    }

    //
    void TVStaticIn()
    {
        TVstatic.gameObject.SetActive(true);     
        StartCoroutine(WaitForFrame(() => TVstatic.SetTrigger("StaticIn")));
    }
    public void TVStaticOut(int sceneIndex)
    {
        selectedScene = sceneIndex;

        Time.timeScale = 1;
        current = Transitions.tvStatic;
        TVstatic.gameObject.SetActive(true);

        TVstatic.SetTrigger("StaticOut");
    }

    //
    void TVOffIn()
    {
        Debug.Log("ENTERING");
        TVOff.gameObject.SetActive(true);
        StartCoroutine(WaitForFrame(() => TVOff.SetTrigger("TVON")));
    }
    public void TVOffOut(int sceneIndex)
    {
        selectedScene = sceneIndex;

        Time.timeScale = 1;
        current = Transitions.tvOff;

        TVOff.gameObject.SetActive(true);
        TVOff.SetTrigger("TVOFF");      
    }

    //
 //   public void EndAnimationsEvents()
 //   {
 //       if (TVOff.GetCurrentAnimatorStateInfo(0).IsName("TV_ON"))
 //       {
 //           TVOff.gameObject.SetActive(false);
 //           
 //       }
 //       else if (TVOff.GetCurrentAnimatorStateInfo(0).IsName("TV_OFF"))
 //       {
 //           SceneManager.LoadScene(selecetedScene);
 //       }
 //       else if (TVstatic.GetCurrentAnimatorStateInfo(0).IsName("STATIC_OUT"))
 //       {
 //           SceneManager.LoadScene(selecetedScene);
 //       }
 //       else if (TVstatic.GetCurrentAnimatorStateInfo(0).IsName("STATIC_IN"))
 //       {
 //           TVstatic.gameObject.SetActive(false);
 //       }
 //   }


   // IEnumerator SceneTimer(float time, int sceneIndex)
   // {
   //     float t = 0;
   //
   //     while(t < time)
   //     {
   //         t += Time.deltaTime;
   //         yield return null;
   //     }
   //
   //     SceneManager.LoadScene(sceneIndex);
   // }
    IEnumerator Timer(float time, Action action)
    {
        float t = 0;
   
        while (t < time)
        {
            t += Time.deltaTime;
            yield return null;
        }
   
        action();
    }

    IEnumerator WaitForFrame(Action action)
    {
        yield return new WaitForEndOfFrame();
        action();
    }
}
