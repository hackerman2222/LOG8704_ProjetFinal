using UnityEngine;

public class PunchRecognizer : MonoBehaviour
{
    public Transform hand;
    private Vector3 lastPos;
    private Vector3 velocity;

    public float jabThreshold = 1.0f;
    public float crossThreshold = 2.0f;
    public float hookThreshold = 1.2f;
    public float uppercutThreshold = 1.0f;

    void Start()
    {
        lastPos = hand.position;
    }

    void Update()
    {
        Vector3 currentPos = hand.position;
        velocity = (currentPos - lastPos) / Time.deltaTime;
        lastPos = currentPos;

        RecognizePunch();
    }

    void RecognizePunch()
    {
        if (velocity.magnitude < 1f) return; // ignore tiny motion

    }

    void UpdateDebug()
    {
        if (VRTextDebug.Instance == null) return;

        VRTextDebug.Instance.Set(
            "Vel: " + velocity.magnitude.ToString("F2") + "\n" +
            "FDot: " + Vector3.Dot(velocity.normalized, hand.forward).ToString("F2") + "\n" +
            "RDot: " + Vector3.Dot(velocity.normalized, hand.right).ToString("F2") + "\n" +
            "UDot: " + Vector3.Dot(velocity.normalized, hand.up).ToString("F2")
        );
    }

}
