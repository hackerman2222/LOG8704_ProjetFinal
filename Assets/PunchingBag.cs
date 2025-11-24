using UnityEngine;

public class PunchingBag : MonoBehaviour
{
    public float forceMultiplier = 4f;
    public Rigidbody rb;

    void OnTriggerEnter(Collider other)
    {
        Rigidbody handRb = other.attachedRigidbody;
        if (handRb != null)
        {
            Vector3 punchVelocity = new Vector3(1,1,1);
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            rb.AddForceAtPosition(punchVelocity * forceMultiplier, hitPoint, ForceMode.Impulse);
        }
    }
}