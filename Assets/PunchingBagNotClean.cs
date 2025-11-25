using UnityEngine;
using Oculus.Interaction.Input;
using Meta.XR.MRUtilityKit;
using System.Threading.Tasks;
using UnityEngine.XR;

public class PunchingBagPlacerNottClean : MonoBehaviour
{
    public GameObject punchingBagPrefab;
    public GameObject punchingBagPreview;
    public Transform rightHandAnchor; 
    public LayerMask floorLayer;
    public float maxDistance = 10f;

    // Visual ray objects
    public LineRenderer lineRenderer;
    public GameObject hitIndicator;

    public float heightAdjust = 1.25f;

    private Vector3 punchingBagPlacement;
    private GameObject currentPreview;

    void Start()
    {
        if (punchingBagPrefab != null) 
        {
            currentPreview = Instantiate(punchingBagPreview);
            currentPreview.SetActive(false);
        }
        // Configure LineRenderer if assigned
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = true;
        }

        if (hitIndicator != null)
        {
            hitIndicator.SetActive(false);
            VRTextDebug.Instance.Set("FKYOU");
        }
    }

    void Update()
    {
        Ray ray = new Ray(rightHandAnchor.position, rightHandAnchor.forward);

        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        bool hitBag = Physics.Raycast(rightHandAnchor.position, rightHandAnchor.forward, out RaycastHit bagHit, maxDistance);
        bool hitSomething = room.Raycast(ray, maxDistance, out RaycastHit hit, out MRUKAnchor anchor);

        if (hitBag && bagHit.collider.CompareTag("Bag")) 
        {
            lineRenderer.SetPosition(0, rightHandAnchor.position);
            lineRenderer.SetPosition(1, bagHit.point);
            hitSomething = false;
        }
        
        // --- Draw Ray ---
        if (lineRenderer != null && !(hitBag && bagHit.collider.CompareTag("Bag")))
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

        // --- Spawn Punching Bag preview---
        if (hitSomething && 
            anchor.AnchorLabels[0] == "FLOOR")
        {
            if (punchingBagPreview != null)
            {
                currentPreview.transform.position = new Vector3(hit.point.x, hit.point.y + heightAdjust, hit.point.z);
                currentPreview.transform.rotation = Quaternion.LookRotation(hit.normal);
                currentPreview.SetActive(true);
            }
        }
        else 
        {
            if (punchingBagPreview != null)
            {
                punchingBagPreview.SetActive(false);
            }
        }

        // --- Spawn Punching Bag ---
        if (hitSomething && 
            anchor.AnchorLabels[0] == "FLOOR" &&
            OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            punchingBagPlacement = new Vector3(hit.point.x, hit.point.y + heightAdjust, hit.point.z);
            Instantiate(punchingBagPrefab, punchingBagPlacement, Quaternion.identity);
        }
    }

    void OnDisable()
    {
        if (hitIndicator != null)
        {
            hitIndicator.SetActive(false);
        }

        if (punchingBagPreview != null)
        {
            currentPreview.SetActive(false);
        }
    }
}
