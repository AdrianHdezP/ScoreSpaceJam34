using UnityEngine;

public class PickUp : MonoBehaviour
{
    private DeliveryManager deliveryManager;
    private AudioSource audioSource;

    private void Awake()
    {
        deliveryManager = FindFirstObjectByType<DeliveryManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BallController player))
        {
            if (deliveryManager.currentState == State.PickUpPending)
            {
                deliveryManager.CompletePickUp();
                audioSource.Play();
            }
        }
    }
}
