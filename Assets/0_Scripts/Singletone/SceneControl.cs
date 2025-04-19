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
    [SerializeField] GameObject TVstatic;
    [SerializeField] Animator TVOff;

    public GameMaster gM;

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
        else TVstatic.SetActive(false);

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
        current = Transitions.fade;

        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        fadeImage.DOFade(1, 2).SetEase(Ease.InOutQuad).OnComplete(()=>SceneManager.LoadScene(sceneIndex));
    }

    void TVStaticIn()
    {
        TVstatic.SetActive(true);

        UnityEvent _event = new UnityEvent();
        _event.AddListener(() => TVstatic.SetActive(false));
        TVOff.GetComponent<AnimationEventDispatcher>().OnAnimationComplete = _event;
    }
    public void TVStaticOut(int sceneIndex)
    {
        current = Transitions.tvStatic;
        TVstatic.SetActive(true);

        UnityEvent _event = new UnityEvent();
        _event.AddListener(() => SceneManager.LoadScene(sceneIndex));
        TVOff.GetComponent<AnimationEventDispatcher>().OnAnimationComplete = _event;
    }

    void TVOffIn()
    {
        TVOff.gameObject.SetActive(true);

        TVOff.SetTrigger("TVON");

        UnityEvent _event = new UnityEvent();
        _event.AddListener(() => TVOff.gameObject.SetActive(false));
        TVOff.GetComponent<AnimationEventDispatcher>().OnAnimationComplete = _event;
    }
    public void TVOffOut(int sceneIndex)
    {
        current = Transitions.tvOff;

        TVOff.gameObject.SetActive(true);
        TVOff.SetTrigger("TVOFF");

        UnityEvent _event = new UnityEvent();
        _event.AddListener(() => SceneManager.LoadScene(sceneIndex));
        TVOff.GetComponent<AnimationEventDispatcher>().OnAnimationComplete = _event;
    }


    IEnumerator SceneTimer(float time, int sceneIndex)
    {
        float t = 0;

        while(t < time)
        {
            t += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneIndex);
    }
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
}
