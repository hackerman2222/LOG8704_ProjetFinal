using UnityEngine;

public class HandColliderVisualizer : MonoBehaviour
{
    public Color debugColor = new Color(1, 0, 0, 0.4f); // red transparent

    private Collider col;
    private MeshRenderer vis;

    void Start()
    {
        col = GetComponent<SphereCollider>(); // assign to class variable
        if (!col) return;

        GameObject visualGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        visualGO.transform.SetParent(this.transform);
        visualGO.transform.localPosition = col.bounds.center - transform.position;
        visualGO.transform.localScale = Vector3.one * col.bounds.size.x; // roughly match radius

        // Make it transparent
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.color = debugColor;
        mat.SetFloat("_Surface", 1);   // transparent
        mat.SetFloat("_Blend", 0);

        vis = visualGO.GetComponent<MeshRenderer>(); // assign to class variable
        vis.material = mat;

        Destroy(visualGO.GetComponent<Collider>()); // keep only visuals
    }

    void Update()
    {
        bool isOpen = GlobalSettings.Instance.IsMenuOpen;
        if (isOpen)
        {
            DisableColliders();
        }
    }

    public void DisableColliders()
    {
        if (col != null)
            col.enabled = false;

        if (vis != null)
            vis.enabled = false;
    }

    public void ActivateColliders()
    {
        if (col != null)
            col.enabled = true;

        if (vis != null)
            vis.enabled = true;
    }
}
