using UnityEngine;
using Oculus.Interaction.Input;
using Meta.XR.MRUtilityKit;
using System.Threading.Tasks;
using UnityEngine.XR;

public class PunchingBagPlacer : MonoBehaviour
{
    public GameObject punchingBagPrefab;
    public GameObject punchingBagPreview;
    public GameObject punchingBagGreen;
    public Transform rightHandAnchor;
    public Transform leftHandAnchor; 
    public LayerMask floorLayer;
    public float maxDistance = 10f;
    public AudioSource audioSource;
    public AudioClip clickClip;

    // Visual ray objects
    public LineRenderer lineRenderer;
    public GameObject hitIndicator;

    public float heightAdjust = 1.25f;

    private Vector3 punchingBagPlacement;
    private Transform handAnchor;
    private GameObject redPreview;
    private GameObject greenPreview;
    private GameObject selectedBag = null;
    private bool canPlace = false;

    void Start()
    {
        if (punchingBagPreview != null) 
        {
            redPreview = Instantiate(punchingBagPreview);
            redPreview.SetActive(false);
        }

        if (punchingBagGreen != null) 
        {
            greenPreview = Instantiate(punchingBagGreen);
            greenPreview.SetActive(false);
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
        }
    }

    void Update()
    {
        bool isMenuOpened = GlobalSettings.Instance.IsMenuOpen;
        bool IsLeftHanded = GlobalSettings.Instance.IsLeftHanded;
        if (isMenuOpened)
        {
            if (lineRenderer != null) lineRenderer.enabled = false;
            if (hitIndicator != null) hitIndicator.SetActive(false);

            if (redPreview != null) redPreview.SetActive(false);
            if (greenPreview != null) greenPreview.SetActive(false);
            return;
        }

        if (lineRenderer != null) lineRenderer.enabled = true;
        if (hitIndicator != null) hitIndicator.SetActive(true);

        if (IsLeftHanded) 
        {
            handAnchor = leftHandAnchor;
        }
        else 
        {
            handAnchor = rightHandAnchor;
        }

        Ray ray = new Ray(handAnchor.position, handAnchor.forward);

        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        bool hitBag = Physics.Raycast(handAnchor.position, handAnchor.forward, out RaycastHit bagHit, maxDistance);
        bool hitSomething = room.Raycast(ray, maxDistance, out RaycastHit hit, out MRUKAnchor anchor);

        if (hitBag && bagHit.collider.CompareTag("Bag")) 
        {
            lineRenderer.SetPosition(0, handAnchor.position);
            lineRenderer.SetPosition(1, bagHit.point);
            hitSomething = false;

            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch) || 
            OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch)) 
            {
                if (selectedBag == bagHit.collider.gameObject) 
                {
                    SetBagOutline(selectedBag, false); 
                    selectedBag = null;
                }
                else 
                {
                    if (selectedBag != null)
                    {
                        SetBagOutline(selectedBag, false); 
                    }
                    selectedBag = bagHit.collider.gameObject;
                    SetBagOutline(selectedBag, true);
                }
                audioSource.PlayOneShot(clickClip);
            }
        }

        if (selectedBag != null && (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) || OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))) 
        {
            Destroy(selectedBag);
            selectedBag = null;
            audioSource.PlayOneShot(clickClip);
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
            {
                hitIndicator.SetActive(false);
            }

            if (redPreview != null)
            {
                redPreview.SetActive(false);
            }

            if (greenPreview != null)
            {
                greenPreview.SetActive(false);
            }
        }

        // --- Spawn Punching Bag preview---
        if (hitSomething && 
            anchor.AnchorLabels[0] == "FLOOR")
        {
            if (punchingBagPreview != null)
            {
                redPreview.transform.position = new Vector3(hit.point.x, hit.point.y + heightAdjust, hit.point.z);
                redPreview.transform.rotation = Quaternion.identity;
                greenPreview.transform.position = new Vector3(hit.point.x, hit.point.y + heightAdjust, hit.point.z);
                greenPreview.transform.rotation = Quaternion.identity;
                canPlace = CanPlace(redPreview);
                if (canPlace)
                {
                    greenPreview.SetActive(true);
                    redPreview.SetActive(false);
                }
                else
                {
                    redPreview.SetActive(true);
                    greenPreview.SetActive(false);
                }
            }
        }
        else 
        {
            if (redPreview != null)
            {
                redPreview.SetActive(false);
            }

            if (greenPreview != null)
            {
                greenPreview.SetActive(false);
            }
        }

        // --- Spawn Punching Bag ---
        if (hitSomething &&
            canPlace && 
            anchor.AnchorLabels[0] == "FLOOR" &&
            (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) ||
            OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch)))
        {
            punchingBagPlacement = new Vector3(hit.point.x, hit.point.y + heightAdjust, hit.point.z);
            if (selectedBag != null) 
            {
                Destroy(selectedBag);
                selectedBag = Instantiate(punchingBagPrefab, punchingBagPlacement, Quaternion.identity);
                SetBagOutline(selectedBag, true);
            }
            else 
            {
                var newBag = Instantiate(punchingBagPrefab, punchingBagPlacement, Quaternion.identity);
                SetBagOutline(newBag, false);
            }
            audioSource.PlayOneShot(clickClip);
        }
    }

    private bool CanPlace(GameObject preview)
    {
        if (preview == null) return false;

        CapsuleCollider capsule = preview.GetComponent<CapsuleCollider>();
        if (capsule == null) return false;

        // Calculate world-space capsule points
        Vector3 center = capsule.transform.position + capsule.center;
        float radius = capsule.radius * Mathf.Max(
            capsule.transform.lossyScale.x,
            capsule.transform.lossyScale.z); // X/Z for horizontal radius
        float height = capsule.height * capsule.transform.lossyScale.y;

        Vector3 point1 = center + Vector3.up * (height / 2 - radius);
        Vector3 point2 = center - Vector3.up * (height / 2 - radius);

        // Check for overlapping colliders
        Collider[] hits = Physics.OverlapCapsule(
            point1,
            point2,
            radius,
            ~0, // all layers, or use a LayerMask
            QueryTriggerInteraction.Ignore // ignore triggers
        );

        foreach (var hit in hits)
        {
            if (hit.gameObject != preview)
            {
                return false; // something is in the way
            }
        }

        return true; // nothing in the way, safe to place
    }

    private void SetBagOutline(GameObject bag, bool enable)
    {
        Outline outline = bag.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = enable; // turn outline on/off
            if (enable)
            {
                outline.OutlineColor = Color.green; // set the outline color
                outline.OutlineWidth = 10f;          // optional: set width
            }
        }
    }

    void OnDisable()
    {
        if (hitIndicator != null)
        {
            hitIndicator.SetActive(false);
        }

        if (redPreview != null)
        {
            redPreview.SetActive(false);
        }

        if (greenPreview != null)
        {
            greenPreview.SetActive(false);
        }
        SetBagOutline(selectedBag, false); 
        selectedBag = null;
    }
}
