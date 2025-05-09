using UnityEngine;
using UnityEngine.InputSystem;

public class GameMaster : MonoBehaviour
{
    PauseMenu menu;
    public bool paused;
    public bool ended;


    private void Awake()
    {
        menu = FindFirstObjectByType<PauseMenu>();
    }

    public void FreezeGame()
    {
        paused = true;
        Time.timeScale = 0;
    }
    public void UnFreezeGame()
    {
        if (!ended)
        {
            paused = false;
            Time.timeScale = 1;
        }
    }

    public void EndGame()
    {

    }
}
