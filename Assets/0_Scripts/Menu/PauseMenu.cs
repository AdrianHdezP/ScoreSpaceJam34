using DG.Tweening;
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

    public void ActivatePause()
    {
        holder.gameObject.SetActive(true);
        ActivateSection(0);
    }
    public void DeactivatePause()
    {
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
        MainSingletone.inst.sceneControl.FadeOut(2);
    }
}
