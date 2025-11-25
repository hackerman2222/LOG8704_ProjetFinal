using UnityEngine;

public class HandColliderVisualizer : MonoBehaviour
{
    public Color debugColor = new Color(1, 0, 0, 0.4f); // red transparent

    void Start()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        if (!col) return;

        GameObject vis = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        vis.transform.SetParent(this.transform);
        vis.transform.localPosition = col.center;
        vis.transform.localScale = Vector3.one * col.radius * 2f;

        // Make it transparent
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.color = debugColor;
        mat.SetFloat("_Surface", 1);   // transparent
        mat.SetFloat("_Blend", 0);
        vis.GetComponent<MeshRenderer>().material = mat;

        Destroy(vis.GetComponent<Collider>()); // keep only the visuals
    }
}
