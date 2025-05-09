using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Transform[] sections;
    [SerializeField] Image[] buttons;
    [SerializeField] Color unselectedColor;
    [SerializeField] Color selectedColor;
    [SerializeField] Transform holder;

    [SerializeField] InputActionReference toggleMenu;
    [SerializeField] float toggleMenuValue;
    [SerializeField] bool pressed;

    int? currentSection;
    Animator animator;
    [SerializeField] Animator[] menuButtons;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        toggleMenuValue = toggleMenu.action.ReadValue<float>();
        if (toggleMenuValue == 0) pressed = false;

        if (!pressed && toggleMenuValue != 0)
        {
            if (!holder.gameObject.activeSelf) ActivatePause(); 
            else DeactivatePause(); 

            pressed = true;
        }
    }

    public void ActivateSettings()
    {
        ActivateSection(0);
        animator.SetInteger("Settings", 1);
        StartCoroutine(OffsetMenuButtons(2));
    }
    public void DectivateSettings()
    {
        animator.SetInteger("Settings", 2);
        StartCoroutine(OffsetMenuButtons(1));
    }

    public void ActivatePause()
    {
        MainSingletone.inst.sceneControl.gM.FreezeGame();
        holder.gameObject.SetActive(true);

        StartCoroutine(OffsetMenuButtons(1));
    }
    public void DeactivatePause()
    {
        MainSingletone.inst.sceneControl.gM.UnFreezeGame();
        PlayerPrefs.Save();
        holder.gameObject.SetActive(false);
    }

    public void ActivateSection(int index)
    {
        if (index == currentSection) return;

        foreach (var section in sections)
        {
            section.gameObject.SetActive(false);
        }

        foreach(var button in buttons)
        {
            button.color = unselectedColor;
        }

        if (index < sections.Length)
        {
            buttons[index].color = selectedColor;
           
            sections[index].gameObject.SetActive(true);
            currentSection = index;

            BringToFront(sections[index].transform);
            BringToFront(buttons[index].transform);
        }
    }

    public void OpenURl()
    {
        Application.OpenURL("https://games-for-robots.itch.io/");
    }

    void BringToFront(Transform tf)
    {
        tf.SetSiblingIndex(tf.parent.childCount - 1);
    }

    public void GoToGame()
    {
        PlayerPrefs.Save();
        MainSingletone.inst.sceneControl.TVStaticOut(2);
    }
    public void GoToMenu()
    {
        PlayerPrefs.Save();
        MainSingletone.inst.sceneControl.TVStaticOut(1);
    }

    IEnumerator OffsetMenuButtons(int animIndex)
    {
        foreach (Animator anim in menuButtons)
        {
            anim.SetInteger("Menu", animIndex);
            yield return new WaitForSecondsRealtime(0.15f);
        }
    }
}
