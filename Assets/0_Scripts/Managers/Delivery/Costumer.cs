using UnityEngine;

public class Costumer : MonoBehaviour
{
    private DeliveryManager deliveryManager;
    private AudioManager audioManager;

    private void Awake()
    {
        deliveryManager = FindFirstObjectByType<DeliveryManager>();
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BallController player))
        {
            deliveryManager.CompleteDelivery();
            audioManager.PlayOneShoot(audioManager.effectsSource, audioManager.costumer, 0.5f);
        }
    }
}
