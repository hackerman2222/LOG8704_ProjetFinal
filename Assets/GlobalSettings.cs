using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static GlobalSettings Instance;

    public bool IsLeftHanded = false;
    public bool IsMenuOpen = false;
    public bool IsVoiceOn = true;
    public bool IsFreePractice = false;

    void Awake()
    {
        Instance = this;
    }

    public void setHandedness(bool handedness)
    {
        IsLeftHanded = handedness;
    }

    public void setMenuState(bool isOpen)
    {
        IsMenuOpen = isOpen;
    }

    public void ToggleVoice()
    {
        IsVoiceOn = !IsVoiceOn;
    }

    public void setFreePracticeState(bool isPractice)
    {
        IsFreePractice = isPractice;
    }
}