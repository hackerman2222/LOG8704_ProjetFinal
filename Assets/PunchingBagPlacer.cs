using UnityEngine;
using Oculus.Interaction.Input;
using Meta.XR.MRUtilityKit;
using System.Threading.Tasks;
using UnityEngine.XR;

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
        // _ = InitializeSceneAsync();

        SwitchToRoomScale();

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

    private async Task InitializeSceneAsync()
    {

        var result = await MRUK.Instance.LoadSceneFromDevice(
            requestSceneCaptureIfNoDataFound: false,
            removeMissingRooms: true
        );

        if (result == MRUK.LoadDeviceResult.Success)
        {
            //OVRScene.RequestSpaceSetup();
        }
        else 
        {
            OVRScene.RequestSpaceSetup();
        }
    }

    void SwitchToRoomScale()
{
    bool success = XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
    if (success)
        Debug.Log("Switched to Roomscale successfully!");
    else
        Debug.LogWarning("Failed to switch to Roomscale.");
}

    void Update()
    {
        Ray ray = new Ray(rightHandAnchor.position, rightHandAnchor.forward);

        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        bool hitSomething = room.Raycast(ray, maxDistance, out RaycastHit hit, out MRUKAnchor anchor);

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
            anchor.AnchorLabels[0] == "FLOOR" &&
            OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            punchingBagPlacement = new Vector3(hit.point.x, hit.point.y + heightAdjust, hit.point.z);
            Instantiate(punchingBagPrefab, punchingBagPlacement, Quaternion.identity);
        }
    }
}
