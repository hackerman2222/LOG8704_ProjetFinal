using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static GlobalSettings Instance;

    public bool IsLeftHanded = false;

    void Awake()
    {
        Instance = this;
    }

    
    public void setHandedness(bool handedness)
    {
        IsLeftHanded = handedness;
    }
}