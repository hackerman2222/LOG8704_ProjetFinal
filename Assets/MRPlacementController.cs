using UnityEngine;

public class MRPlacementController : MonoBehaviour
{
    [Header("References")]
    public GameObject floorEffectMesh;      // Your floor visualization
    public GameObject otherEffectMesh;      // Your other visualization
    public GameObject punchingBagPlacerGO;  // Parent GameObject holding PunchingBagPlacer script + visuals

    private bool isEditMode = false;

    void Start()
    {
        // Start with everything disabled
        SetEditMode(false);
    }

    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Four, OVRInput.Controller.LTouch)) 
        {
            ToggleEditMode();
        }
    }

    public void SetEditMode(bool enabled)
    {
        isEditMode = enabled;

        if (floorEffectMesh != null)
            floorEffectMesh.SetActive(enabled);

        if (punchingBagPlacerGO != null)
            punchingBagPlacerGO.SetActive(enabled);

        if (otherEffectMesh != null)
            otherEffectMesh.SetActive(enabled);
    }

    public void ToggleEditMode()
    {
        SetEditMode(!isEditMode);
    }
}

