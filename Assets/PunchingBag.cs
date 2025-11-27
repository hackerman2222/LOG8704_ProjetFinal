using UnityEngine;
using UnityEngine.Events;
using Meta.XR.Audio;
using TMPro;

public enum PunchType
{
    None,
    Jab,
    Cross,
    Hook,
    Uppercut
}

public class PunchingBag : MonoBehaviour
{
    public float hapticDuration = 0.15f;
    public Transform textCanvas;

    [Header("Combo Settings")]
    [Tooltip("The time (in seconds) allowed between consecutive hits before the combo resets.")]
    public float comboResetTime = 9f; 

    [Header("Current Combo Data")]
    [Tooltip("The current number of consecutive hits.")]
    [SerializeField]
    private int currentCombo = 0;

    private float lastHitTime;

    [Header("Punch Detection Settings")]
    public float jabForwardDot = 0.5f;      // mostly forward
    public float hookSideDot = 0.6f;         // strong sideways
    public float uppercutUpDot = 0.6f;       // strong upward

    public AudioSource audioSource;
    public AudioClip punchClip;
    public AudioClip crossClip;
    public AudioClip hookClip;
    public AudioClip uppercutClip;

    public AudioClip voicePunchClip;
    public AudioClip voiceCrossClip;
    public AudioClip voiceHookClip;
    public AudioClip voiceUppercutClip;

    private OVRInput.Controller controller;
    private bool IsLeftPunch = false;
    public TextMeshPro comboText;
    private bool IsLeftHanded = false;
    private bool IsVoiceOn = true;


    void Start()
    {
        lastHitTime = 0;
        currentCombo = 0;
        UpdateCombo(currentCombo);
        IsLeftHanded = GlobalSettings.Instance.IsLeftHanded;
    }

    void Update() 
    {
        IsLeftHanded = GlobalSettings.Instance.IsLeftHanded;
        IsVoiceOn = GlobalSettings.Instance.IsVoiceOn;

        float timeSinceLastHit = Time.time - lastHitTime;
        if (timeSinceLastHit > comboResetTime)
        {
            //currentCombo = 0;
            UpdateCombo(currentCombo);
        }

        if (textCanvas != null)
        {
            textCanvas.LookAt(Camera.main.transform);
            textCanvas.Rotate(0,180,0);
        }
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
            IsLeftPunch = true;
        }
        if (collision.collider.CompareTag("Right Controller")) 
        {
            controller = OVRInput.Controller.RTouch;
            IsLeftPunch = false;
        }
        
        Vector3 handVel = OVRInput.GetLocalControllerVelocity(controller);
        Vector3 bagVel = GetComponent<Rigidbody>().linearVelocity;

        // relative velocity is more accurate for punches
        float impactForce = (handVel - bagVel).magnitude;

        // better scaling curve
        float intensity = Mathf.Clamp01(Mathf.Pow(impactForce / 1.5f, 2f)); 

        HapticInteractable.Instance.PlayHaptics(controller, 0.75f, hapticDuration);
        
        DetectPunch(handVel, collision.collider.transform);
        currentCombo++;
        UpdateCombo(currentCombo);
        lastHitTime = Time.time;
    }

    public void PlayPunchSound(AudioClip pounchSound)
    {
        if (audioSource == null || pounchSound == null) return;

        audioSource.PlayOneShot(pounchSound, 2f);
    }

    private void DetectPunch(Vector3 velocity, Transform hand)
    {
        Vector3 dir = velocity.normalized;

        Transform head = Camera.main.transform;
        Vector3 armDir = (hand.position - head.position).normalized;

        float forwardDot = Vector3.Dot(dir, armDir);
        float rightDot   = Mathf.Abs(Vector3.Dot(dir, Vector3.Cross(Vector3.up, armDir)));
        float upDot      = Vector3.Dot(dir, Vector3.up);

        // Jab / Cross = strong forward punch
        if (forwardDot > jabForwardDot)
            {
                if (IsLeftHanded) 
                {
                    if (IsLeftPunch)
                    {
                        PlayPunchSound(crossClip);
                        if (IsVoiceOn) 
                        {
                            PlayPunchSound(voiceCrossClip);
                        }
                    }
                    else 
                    {
                        PlayPunchSound(punchClip);
                        if (IsVoiceOn) 
                        {
                            PlayPunchSound(voicePunchClip);
                        }
                    }
                }
                else 
                {
                    if (IsLeftPunch)
                    {
                        PlayPunchSound(punchClip);
                        if (IsVoiceOn) 
                        {
                            PlayPunchSound(voicePunchClip);
                        }
                    }
                    else 
                    {
                        PlayPunchSound(crossClip);
                        if (IsVoiceOn) 
                        {
                            PlayPunchSound(voiceCrossClip);
                        }
                    }
                }
                return;
            }
        // Hook = strong sideways punch
        if (rightDot > hookSideDot)
            {
                PlayPunchSound(hookClip);
                if (IsVoiceOn) 
                {
                    PlayPunchSound(voiceHookClip);
                }
                return;
            }

        // Uppercut = strong upward motion
        if (upDot > uppercutUpDot)
            {
                PlayPunchSound(uppercutClip);
                if (IsVoiceOn) 
                {
                    PlayPunchSound(voiceUppercutClip);
                }
                return;
            }
        PlayPunchSound(punchClip);
        return;
    }

    void UpdateCombo(int comboCount)
    {
        if (comboCount == 0)
        {
            comboText.text = "COMBO";
        }
        else
        {
            comboText.text = "COMBO: " + comboCount.ToString();;
        }
    }
}