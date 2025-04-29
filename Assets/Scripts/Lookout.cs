using UnityEngine;

// Decrease the volume based on the angle between the camera and the object
public class VolumeBasedOnAngle : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private GameObject cameraGameObject;
    [SerializeField] private AudioSource audioSource;

    [Header("Angle Settings")]
    [SerializeField] private float maxAngle = 50f;  // Max angle where volume becomes 0
    [SerializeField] private AnimationCurve volumeCurve = AnimationCurve.Linear(0, 1, 180, 0);  // Curve for volume change
    
    
    private void Start()
    {
        if (Camera.main != null) cameraGameObject = Camera.main.gameObject;
        
    }

    private void Update()
    {
        Vector3 direction = transform.position - cameraGameObject.transform.position;
        float angle = Vector3.Angle(direction, cameraGameObject.transform.forward);

        // Clamp the angle to the max angle to ensure it doesn't go beyond the threshold
        angle = Mathf.Min(angle, maxAngle);

        // Get the normalized value from the angle based on the curve
        float normalizedAngle = angle / maxAngle;

        // Use the curve to calculate the volume
        float volume = volumeCurve.Evaluate(normalizedAngle);

        // Set the volume of the audio source
        audioSource.volume = volume;
    }
}