using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class LaCanneDePapi : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference leftClickInput;   
    [SerializeField] private InputActionReference rightClickInput;  

    [Header("Haptics")]
    [SerializeField] private HapticImpulsePlayer lefthandHapticImpulsePlayer;
    [SerializeField] private HapticImpulsePlayer righthandHapticImpulsePlayer;

    [Header("Ray Settings")]
    [SerializeField] private float rayLength = 10f;
    [SerializeField] private float minHapticDuration = 0.05f;
    [SerializeField] private float maxHapticDuration = 0.2f;
    [SerializeField] private float hapticInterval = 0.3f;  // Interval between impulses

    private bool isLeftButtonPressed = false;
    private bool isRightButtonPressed = false;

    private float lastLeftHapticTime;
    private float lastRightHapticTime;

    private void OnEnable()
    {
        leftClickInput.action.Enable();
        rightClickInput.action.Enable();

        leftClickInput.action.performed += _ => isLeftButtonPressed = true;
        leftClickInput.action.canceled += _ => isLeftButtonPressed = false;

        rightClickInput.action.performed += _ => isRightButtonPressed = true;
        rightClickInput.action.canceled += _ => isRightButtonPressed = false;
    }

    private void OnDisable()
    {
        leftClickInput.action.Disable();
        rightClickInput.action.Disable();
    }

    private void Update()
    {
        float currentTime = Time.time;

        // Check haptic intervals before firing rays
        if (isLeftButtonPressed && currentTime - lastLeftHapticTime >= hapticInterval)
        {
            FireRay(lefthandHapticImpulsePlayer, Color.red);
            lastLeftHapticTime = currentTime;
        }

        if (isRightButtonPressed && currentTime - lastRightHapticTime >= hapticInterval)
        {
            FireRay(righthandHapticImpulsePlayer, Color.blue);
            lastRightHapticTime = currentTime;
        }
    }

    private void FireRay(HapticImpulsePlayer player, Color rayColor)
    {
        Transform controllerTransform = player.gameObject.transform;
        Ray ray = new Ray(controllerTransform.position, controllerTransform.forward + controllerTransform.up *0.5f);

        // Draw the ray for visualization
        Debug.DrawRay(ray.origin, ray.direction * rayLength, rayColor);

        if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
        {
            
            float distance = hit.distance;

            // Intensity and duration based on proximity
            float intensity = Mathf.Clamp01(1 - (distance / rayLength));
            float duration = Mathf.Lerp(minHapticDuration, maxHapticDuration, intensity);

            // Send haptic impulse
            player.SendHapticImpulse(intensity, duration, 0.5f);
            
            // Play hit object material sound
            Object obj = hit.collider.GetComponent<Object>();
            if (obj != null)
            {
                Mat mat = obj.material;
                switch (mat)
                {
                    case Mat.METAL:
                        AudioManagerController.PlayAudioOnce(Audio.METALSOUND, 0.5f);
                        break;
                    case Mat.STONE:
                        AudioManagerController.PlayAudioOnce(Audio.STONESCRATCH, 0.5f);
                        break;
                    case Mat.WOOD:
                        AudioManagerController.PlayAudioOnce(Audio.WOODSCRATCH, 0.5f);
                        break;
                }
            }

            // Debug info
            Debug.Log($"Haptic Impulse: Intensity = {intensity:F2}, Duration = {duration:F2}, Distance = {distance:F2}");
        }
    }
}
