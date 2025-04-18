using UnityEngine;

public class HandleWheelSmoke : MonoBehaviour
{
    private PlayerController controller;
    private ParticleSystem myParticleSystem;

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
        myParticleSystem = GetComponent<ParticleSystem>();

        myParticleSystem.Stop();
    }

    private void Update()
    {
        bool screeching = controller.IsTireScreeching(out float lateralVelocity, out bool isBracking);

        if (screeching && !myParticleSystem.isPlaying)
            myParticleSystem.Play();
        else if (!screeching && myParticleSystem.isPlaying)
            myParticleSystem.Stop();
    }
}
