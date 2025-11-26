using UnityEngine;

public class PunchRecognizer : MonoBehaviour
{
    public OVRInput.Controller controller;
    public bool isDominantHand = false;

    public float jabThreshold = 1.2f;
    public float crossThreshold = 1.8f;
    public float hookThreshold = 1.0f;
    public float uppercutThreshold = 1.0f;

    void Update()
    {
        Vector3 vel = OVRInput.GetLocalControllerVelocity(controller);

        RecognizePunch(vel);
    }

    void RecognizePunch(Vector3 vel)
    {
        float speed = vel.magnitude;
        if (speed < 0.5f) return;

        // Normalize for direction checks
        Vector3 dir = vel.normalized;

        float forwardDot = Vector3.Dot(dir, transform.forward);
        float rightDot   = Vector3.Dot(dir, transform.right);
        float upDot      = Vector3.Dot(dir, transform.up);

        // ----------- JAB / CROSS -----------
        if (forwardDot > 0.7f && speed > jabThreshold) 
        {
            if (isDominantHand && speed > crossThreshold)
                UpdateDebug(vel);
            else
                UpdateDebug(vel);
            return;
        }

        // ----------- HOOK -----------
        // big horizontal movement
        if (Mathf.Abs(rightDot) > 0.7f && speed > hookThreshold)
        {
            Debug.Log("HOOK!");
            return;
        }

        // ----------- UPPERCUT -----------
        // upward movement
        if (upDot > 0.6f && speed > uppercutThreshold)
        {
            Debug.Log("UPPERCUT!");
            return;
        }
    }

    void UpdateDebug(Vector3 vel)
    {
        if (VRTextDebug.Instance == null) return;

        VRTextDebug.Instance.Set(
            "Vel: " + vel.magnitude.ToString("F2") + "\n" +
            "Forward Dot: " + Vector3.Dot(vel.normalized, transform.forward).ToString("F2") + "\n" +
            "Right Dot: " + Vector3.Dot(vel.normalized, transform.right).ToString("F2") + "\n" +
            "Up Dot: " + Vector3.Dot(vel.normalized, transform.up).ToString("F2")
        );
    }
}
