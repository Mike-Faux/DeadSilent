using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the volume slider

    void Start()
    {
        // Add a listener to the slider's value changed event
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(HandleVolumeChange);
        }
    }

    void HandleVolumeChange(float sliderValue)
    {
        // Convert the slider's value (0 to 100) to the audio volume range (0.00 to 0.25)
        float volume = Map(sliderValue, 0, 100, 0.00f, 0.25f);

        // Update the audio listener's volume
        AudioListener.volume = volume;
    }

    // Utility method for mapping one range of values to another
    float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}
