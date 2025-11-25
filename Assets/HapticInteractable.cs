using UnityEngine;
using System.Collections;

public class HapticInteractable : MonoBehaviour
{
    public static HapticInteractable Instance;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Plays haptics on a controller.
    /// amplitude = 0 to 1
    /// duration = seconds
    /// </summary>
    public void PlayHaptics(OVRInput.Controller controller, float amplitude, float duration)
    {
        StartCoroutine(HapticRoutine(controller, amplitude, duration));
    }

    private System.Collections.IEnumerator HapticRoutine(OVRInput.Controller controller, float amplitude, float duration)
    {
        OVRInput.SetControllerVibration(1f, amplitude, controller); // freq, amp

        yield return new WaitForSeconds(duration);

        OVRInput.SetControllerVibration(0, 0, controller); // stop
    }
}