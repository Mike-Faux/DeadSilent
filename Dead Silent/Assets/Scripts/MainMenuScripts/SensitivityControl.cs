using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityControl : MonoBehaviour
{
    public Slider sensitivitySlider; // Reference to the sensitivity slider
    // Assuming you have a way to set the actual sensitivity in your player script
    // For example, a reference to the player or a static setting class

    void Start()
    {
        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.AddListener(HandleSensitivityChange);
        }
    }

    void HandleSensitivityChange(float sliderValue)
    {
        // Map the slider's value (1 to 50) to the sensitivity range (0 to 300)
        float sensitivity = Map(sliderValue, 1, 50, 0, 300);
        // Update the player's sensitivity setting here
        // For example: PlayerSettings.MouseSensitivity = sensitivity;
        Debug.Log("Mapped Sensitivity: " + sensitivity);
    }

    // Utility method for mapping one range of values to another
    float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}
