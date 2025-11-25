using UnityEngine;
using TMPro;

public class VRTextDebug : MonoBehaviour
{
    public static VRTextDebug Instance;
    public TextMeshProUGUI text;

    void Awake() => Instance = this;

    public void Set(string msg)
    {
        text.text = msg;
    }
}