using UnityEngine;
using UnityEngine.InputSystem;

public class GameMaster : MonoBehaviour
{
    PauseMenu menu;


    private void Awake()
    {
        menu = FindFirstObjectByType<PauseMenu>();
    }
}
