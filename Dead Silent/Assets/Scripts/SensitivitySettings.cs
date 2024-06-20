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

        // Initialize slider value with current sensitivity
        if (player != null && player.cameraController != null)
        {
            slider.value = player.cameraController.sensitivity;
        }
        else
        {
            Debug.LogError("PlayerScript or CameraController reference is missing.");
        }
    }

    public void HandleSliderValueChanged(float value)
    {
        if (player != null && player.cameraController != null)
        {
            player.cameraController.SetSensitivity(value);
        }
    }
}
