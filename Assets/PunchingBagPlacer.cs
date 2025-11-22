using UnityEngine;
using Oculus.Interaction.Input;

public class PunchingBagPlacer : MonoBehaviour
{
    public GameObject punchingBagPrefab;
    public Transform rightHandAnchor; 
    public LayerMask floorLayer;
    public float maxDistance = 10f;

    // Visual ray objects
    public LineRenderer lineRenderer;
    public GameObject hitIndicator;

    public float heightAdjust = 0.5f;

    private Vector3 punchingBagPlacement;

    void Start()
    {
        // Configure LineRenderer if assigned
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = true;
        }

        if (hitIndicator != null)
        {
            hitIndicator.SetActive(false);
        }
    }

    void Update()
    {
        Ray ray = new Ray(rightHandAnchor.position, rightHandAnchor.forward);
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, maxDistance, floorLayer);

        // --- Draw Ray ---
        if (lineRenderer != null)
        {
            Vector3 endPoint = hitSomething ? hit.point : ray.origin + ray.direction * maxDistance;
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, endPoint);
        }

        // --- Hit Indicator ---
        if (hitSomething)
        {
            if (hitIndicator != null)
            {
                hitIndicator.SetActive(true);
                hitIndicator.transform.position = hit.point;
                hitIndicator.transform.rotation = Quaternion.LookRotation(hit.normal);
            }
        }
        else
        {
            if (hitIndicator != null)
                hitIndicator.SetActive(false);
        }

        // --- Spawn Punching Bag ---
        if (hitSomething &&
            OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            punchingBagPlacement = new Vector3(hit.point.x, hit.point.y + heightAdjust, hit.point.z);
            Instantiate(punchingBagPrefab, punchingBagPlacement, Quaternion.identity);
        }
    }
}
