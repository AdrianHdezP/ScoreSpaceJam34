using UnityEngine;

public class Slime : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
            player.decelerate = true;

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
            enemy.decelerate = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
            player.decelerate = false;


        if (collision.gameObject.TryGetComponent(out Enemy enemy))
            enemy.decelerate = false;
    }
}
