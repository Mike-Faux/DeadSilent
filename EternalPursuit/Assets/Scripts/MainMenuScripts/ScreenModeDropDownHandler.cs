using UnityEngine;
using TMPro; // Use TMP namespace

public class ScreenModeDropdownHandler : MonoBehaviour
{
    public TMP_Dropdown screenModeDropdown; // Use TMP_Dropdown type

    void Start()
    {
        if (screenModeDropdown == null)
        {
            screenModeDropdown = GetComponent<TMP_Dropdown>();
        }

        screenModeDropdown.onValueChanged.AddListener(delegate {
            ToggleScreenMode(screenModeDropdown.value);
        });
    }

    void ToggleScreenMode(int modeIndex)
    {
        switch (modeIndex)
        {
            case 0: // Fullscreen
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Debug.Log("Switched to Fullscreen Mode");
                break;
            case 1: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Debug.Log("Switched to Windowed Mode");
                break;
            default:
                Debug.LogError("Unsupported screen mode selected.");
                break;
        }

        // Log the current screen mode and resolution for verification
        Debug.Log($"Current Screen Mode: {(Screen.fullScreen ? "Fullscreen" : "Windowed")}, Resolution: {Screen.width}x{Screen.height}");
    }
}
