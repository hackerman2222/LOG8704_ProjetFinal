using UnityEngine;

public class PunchingBag : MonoBehaviour
{
    public float forceMultiplier = 4f;

    public Rigidbody rb;

    void OnCollisionEnter(Collision collision)
    {
        // Apply force where the punch hits
        rb.AddForceAtPosition(
            collision.relativeVelocity * forceMultiplier,
            collision.contacts[0].point,
            ForceMode.Impulse
        );
    }
}