using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class SensitivitySlider : MonoBehaviour
{
    public Player player; // Assign this in the editor
    private Slider slider; // Reference to the slider component

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(HandleSliderValueChanged);

        // Initialize slider value based on current sensitivity, mapping from sensitivity range to slider range
        if (player != null && player.cameraController != null)
        {
            // Assuming sensitivity is initially within the range 0 to 300, map this to 1 to 50 for the slider
            slider.value = MapSensitivityToSlider(player.cameraController.sensitivity);
        }
        else
        {
            Debug.LogError("Player or CameraController reference is missing.");
        }
    }

    public void HandleSliderValueChanged(float sliderValue)
    {
        if (player != null && player.cameraController != null)
        {
            // Convert the slider value back to the sensitivity range before setting it
            float sensitivity = MapSliderToSensitivity(sliderValue);
            player.cameraController.SetSensitivity(sensitivity);
        }
    }

    // Map the sensitivity (0 to 300) to the slider's scale (1 to 50)
    private float MapSensitivityToSlider(float sensitivity)
    {
        return (sensitivity / 300f) * 49f + 1f; // Adjusted formula for mapping
    }

    // Map the slider's value (1 to 50) back to the sensitivity range (0 to 300)
    private float MapSliderToSensitivity(float sliderValue)
    {
        return (sliderValue - 1f) / 49f * 300f; // Adjusted formula for reverse mapping
    }
}
