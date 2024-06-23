using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SliderValueDisplay : MonoBehaviour
{
    public Slider slider; // Reference to the Slider component
    public TextMeshProUGUI displayText; // Reference to the TextMeshProUGUI component

    void Start()
    {
        // Ensure there's a reference to the slider and displayText
        if (slider != null && displayText != null)
        {
            // Add a listener to the slider's value changed event
            slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        }
    }

    // Method called whenever the slider's value changes
    void ValueChangeCheck()
    {
        // Update the displayText to show the slider's current value
        // Use ToString() to convert the float value to a string
        // You can format the string to your preference
        displayText.text = slider.value.ToString("0");
    }
}
