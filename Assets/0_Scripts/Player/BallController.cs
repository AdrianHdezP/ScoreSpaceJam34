using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController playerSC;
    private DeliveryManager deliveryManager;
    private AudioManager audioManager;

    public AudioSource hitSource;
    public AudioSource comboSource;

    [Header("Combo visuals")]
    [SerializeField] ParticleSystem[] comboTrailsL;
    [SerializeField] ParticleSystem[] comboTrailsR;

    [Header("Setup")]
    [SerializeField] Material m_brewMat;
    [SerializeField] Material m_particleMat;
    [SerializeField] ParticleSystem smokeParticles;
    [SerializeField] TrailRenderer trailRenderer;

    [SerializeField, ColorUsage(true, true)] Color noneColor;

    [SerializeField] GameObject firePrefab;
    [SerializeField, ColorUsage(true, true)] Color fireColor;

    [SerializeField] GameObject slimePrefab;
    [SerializeField, ColorUsage(true, true)] Color slimeColor;

    [SerializeField] GameObject protectionPrefab;
    [SerializeField, ColorUsage(true, true)] Color protectionColor;

    //private DeliveryType currentDeliveryType;


    [Header("Settings")]
    [SerializeField] private Transform model;
    [SerializeField] private float fireEmissionThreshold;
    [SerializeField] private float speedBoostThreshold;
    [SerializeField] private float spawnDistance;

    float horizontalVelocity;
    Vector2? lastPos;
    Quaternion modelRot;

    [HideInInspector] public int combo;
    private float pitch = 1f;
    float comboT;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSC = FindFirstObjectByType<PlayerController>();
        deliveryManager = FindFirstObjectByType<DeliveryManager>();
        audioManager = FindFirstObjectByType<AudioManager>();

        modelRot = model.localRotation;
    }
    private void Update()
    {
        horizontalVelocity = Vector2.Dot(transform.right, rb.linearVelocity);
        if (Mathf.Abs(horizontalVelocity) < speedBoostThreshold || !playerSC.drifting)
        {
            if (comboT < 0)
            {
                combo = 0;
                pitch = 1;
            }
            else
            {
                comboT -= Time.deltaTime;
            }
        }

        VisualControl();

        if (deliveryManager.currentDeliveryType == DeliveryType.None) return;

        if (Mathf.Abs(horizontalVelocity) > fireEmissionThreshold && (lastPos == null || Vector2.Distance(transform.position, (Vector2)lastPos) >= spawnDistance) && playerSC.drifting)
        {
            lastPos = transform.position;

            if (deliveryManager.currentDeliveryType == DeliveryType.Fire)
                Instantiate(firePrefab, transform.position, Quaternion.identity);

            else if (deliveryManager.currentDeliveryType == DeliveryType.Slime)
                Instantiate(slimePrefab, transform.position, Quaternion.identity);

            else if (deliveryManager.currentDeliveryType == DeliveryType.Protection)
                Instantiate(protectionPrefab, transform.position, Quaternion.identity);
        }
        if (Mathf.Abs(horizontalVelocity) < fireEmissionThreshold) lastPos = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Damageable damagable) && Mathf.Abs(horizontalVelocity) >= speedBoostThreshold && !damagable.recentlyImpacted)
        {
            if (!hitSource.isPlaying)
            {
                float randomPitch = Random.Range(0.75f, 1.25f);
                hitSource.pitch = randomPitch;
                hitSource.Play();
            }

            if (!collision.gameObject.TryGetComponent(out Enemy enemy)|| !enemy.isCharging)
            {
                damagable.RecieveDamage (10, -collision.contacts[0].normal * collision.relativeVelocity.magnitude, true);

                if (playerSC.drifting)
                {
                    comboT = playerSC.comboTimer;
                    comboSource.pitch = pitch;
                    pitch += 0.05f;
                    comboSource.Play();
                    combo++;
                }
            }
        }
    }

    void VisualControl()
    {
        for (int i = 0; i < comboTrailsL.Length; i++)
        {
            if (combo == i + 1 || (combo >= comboTrailsL.Length && i == comboTrailsL.Length - 1))
            {
                comboTrailsL[i].gameObject.SetActive(true);
                comboTrailsR[i].gameObject.SetActive(true);
            }
            else
            {
                comboTrailsL[i].gameObject.SetActive(false);
                comboTrailsR[i].gameObject.SetActive(false);
            }
        }

        float angle = Mathf.Clamp(horizontalVelocity * 15f, -15, 15);
        model.localRotation = modelRot * Quaternion.AngleAxis(angle, -Vector3.right);

        if (Mathf.Abs(horizontalVelocity) > speedBoostThreshold)
        {
            trailRenderer.enabled = true;
        }
        else
        {
            trailRenderer.enabled = false;
        }

        if (deliveryManager.currentDeliveryType == DeliveryType.None)
        {
            m_brewMat.SetColor("_GlowColor", noneColor);
            smokeParticles.Stop();

            return;
        }
        else if (!smokeParticles.isPlaying)
        {
            smokeParticles.Play();
        }

        if (deliveryManager.currentDeliveryType == DeliveryType.Fire)
        {
            m_brewMat.SetColor("_GlowColor", fireColor);
            m_particleMat.SetColor("_GlowColor", fireColor);
        }
           
        else if (deliveryManager.currentDeliveryType == DeliveryType.Slime)
        {
            m_brewMat.SetColor("_GlowColor", slimeColor);
            m_particleMat.SetColor("_GlowColor", slimeColor);
        }

        else if (deliveryManager.currentDeliveryType == DeliveryType.Protection)
        {
            m_brewMat.SetColor("_GlowColor", protectionColor);
            m_particleMat.SetColor("_GlowColor", protectionColor);
        }
    }
}
