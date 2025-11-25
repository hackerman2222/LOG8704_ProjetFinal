using UnityEngine;

public class PunchingBag : MonoBehaviour
{
    public float hapticDuration = 0.15f;
    private OVRInput.Controller controller;

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
        
        float impactForce = collision.relativeVelocity.magnitude;
        float intensity = Mathf.Clamp01(impactForce / 2.5f); 

        HapticInteractable.Instance.PlayHaptics(controller, intensity, hapticDuration);
    }
}