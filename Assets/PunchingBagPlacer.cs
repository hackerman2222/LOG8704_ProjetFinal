using UnityEngine;
using Oculus.Interaction.Input;

public class PunchingBagPlacer : MonoBehaviour
{
    public GameObject punchingBagPrefab;
    public Transform rightHandAnchor; // Assign RightHandAnchor
    public LayerMask floorLayer;      // Optional: set if you have floor colliders
    public float maxDistance = 5f;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Ray ray = new Ray(rightHandAnchor.position, rightHandAnchor.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, floorLayer))
            {
                Instantiate(punchingBagPrefab, hit.point, Quaternion.identity);
            }
            else
            {
                // fallback: spawn at ground level in front of player
                Vector3 pos = rightHandAnchor.position + rightHandAnchor.forward * 2f;
                pos.y = 0f;
                Instantiate(punchingBagPrefab, pos, Quaternion.identity);
            }
        }
    }
}
