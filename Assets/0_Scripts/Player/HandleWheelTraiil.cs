using UnityEngine;

public class HandleWheelTraiil : MonoBehaviour
{
    private PlayerController controller;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.emitting = false;
    }

    private void Update()
    {
        if (controller.IsTireScreeching(out float lateralVelocity, out bool isBracking))
            trailRenderer.emitting = true;
        else
            trailRenderer.emitting = false;
    }
}
