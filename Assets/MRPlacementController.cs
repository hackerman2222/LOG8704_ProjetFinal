using UnityEngine;
using Meta.XR.MRUtilityKit;

public class MRPlacementController : MonoBehaviour
{
    public GameObject punchingBagPlacerGO;
    public EffectMesh mesh1;
    public EffectMesh mesh2;


    private bool isEditMode = false;

    void Start()
    {
        SetEditMode(false); // start disabled
    }

    void Update()
    {
        
    }

    public void SetEditMode(bool enabled)
    {
        isEditMode = enabled;

        mesh1.ToggleEffectMeshVisibility(enabled);
        mesh2.ToggleEffectMeshVisibility(enabled);

        // -----------------------------
        // Punching bag placement system
        // -----------------------------
        if (punchingBagPlacerGO != null)
            punchingBagPlacerGO.SetActive(enabled); 
    }

    public void ToggleEditMode()
    {
        SetEditMode(!isEditMode);
    }
}
