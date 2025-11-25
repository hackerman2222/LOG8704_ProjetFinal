using UnityEngine;
using UnityEngine.Events;
using Meta.XR.Audio;

public class PunchingBag : MonoBehaviour
{
    public float hapticDuration = 0.15f;

    [Header("Combo Settings")]
    [Tooltip("The time (in seconds) allowed between consecutive hits before the combo resets.")]
    public float comboResetTime = 0.8f; 

    [Header("Current Combo Data")]
    [Tooltip("The current number of consecutive hits.")]
    [SerializeField]
    private int currentCombo = 0;

    private float lastHitTime;

    [Header("Events")]
    public UnityEvent<int> OnComboUpdate;

    public AudioSource audioSource;
    public AudioClip punchClip;

    private OVRInput.Controller controller;

    void Start()
    {

        lastHitTime = Time.time;
        OnComboUpdate.Invoke(currentCombo); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Left Controller") &&
            !collision.collider.CompareTag("Right Controller"))
        {
            return; 
        }
        if (collision.collider.CompareTag("Left Controller")) 
        {
            controller = OVRInput.Controller.LTouch;
        }
        if (collision.collider.CompareTag("Right Controller")) 
        {
            controller = OVRInput.Controller.RTouch;
        }
        
        Vector3 handVel = OVRInput.GetLocalControllerVelocity(controller);
        float impactForce = handVel.magnitude;
        float intensity = Mathf.Clamp01(impactForce / 2.5f); 

        HapticInteractable.Instance.PlayHaptics(controller, intensity, hapticDuration);
        PlayPunchSound(intensity);


        // Combo 
        float timeSinceLastHit = Time.time - lastHitTime;

        if (timeSinceLastHit > comboResetTime)
        {
            // reset le combo si pas assez rapide 
            currentCombo = 1; // Reset à 1 parceque c'est le coup qui compte en starter
            Debug.Log($" New Combo Started: {currentCombo}");
        }
        else
        {
            //continue le combo si le coup est assez rapide
            currentCombo++;
            Debug.Log($"Combo Count: {currentCombo}");
        }

        // --- Update ---
        lastHitTime = Time.time;
        
        OnComboUpdate.Invoke(currentCombo);
    }

    public void PlayPunchSound(float intensity)
    {
        if (audioSource == null || punchClip == null) return;

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(punchClip, intensity);
    }
}