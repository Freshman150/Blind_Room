using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class VRProximityHaptics : MonoBehaviour
{
    [Header("Controller References")]
    public Transform leftController;
    public Transform rightController;

    [Header("Proximity Spheres")]
    public float outerSphereRadius = 1.0f;
    public float innerSphereRadius = 0.2f;

    [Header("Haptics")]
    [SerializeField] private HapticImpulsePlayer leftHapticPlayer;
    [SerializeField] private HapticImpulsePlayer rightHapticPlayer;
    [SerializeField] private float minHapticDuration = 0.05f;
    [SerializeField] private float maxHapticDuration = 0.2f;
    [SerializeField] private float hapticInterval = 0.3f; // Interval between impulses
    [SerializeField] private float continuousHapticIntensity = 1.0f; // Full intensity for continuous mode
    [SerializeField] private float continuousHapticDuration = 0.1f; // Short bursts to keep vibrating

    [Header("Visual Feedback")]
    public GameObject leftInnerSphereVisual; // Assign a red sphere for the left hand
    public GameObject rightInnerSphereVisual; // Assign a red sphere for the right hand

    private float lastLeftHapticTime;
    private float lastRightHapticTime;

    private bool leftInsideInnerSphere = false;
    private bool rightInsideInnerSphere = false;

    private void Update()
    {
        ProcessProximity(leftController, leftHapticPlayer, leftInnerSphereVisual, ref lastLeftHapticTime, ref leftInsideInnerSphere);
        ProcessProximity(rightController, rightHapticPlayer, rightInnerSphereVisual, ref lastRightHapticTime, ref rightInsideInnerSphere);
    }

    private void ProcessProximity(Transform controller, HapticImpulsePlayer hapticPlayer, GameObject innerSphereVisual, ref float lastHapticTime, ref bool isInsideInnerSphere)
    {
        Collider[] hitColliders = Physics.OverlapSphere(controller.position, outerSphereRadius);
        float closestDistance = float.MaxValue;
        bool isTouchingInnerSphere = false;

        foreach (var col in hitColliders)
        {
            if (col.gameObject == controller.gameObject) continue; // Ignore self-collisions

            float distance = Vector3.Distance(controller.position, col.ClosestPoint(controller.position));

            if (distance <= innerSphereRadius)
            {
                isTouchingInnerSphere = true;
                break; // No need to check further, max intensity reached
            }

            if (distance < closestDistance)
            {
                closestDistance = distance;
            }
        }

        if (isTouchingInnerSphere)
        {
            // Continuous full vibration when inside the small sphere
            if (!isInsideInnerSphere) // Start vibration when entering
            {
                hapticPlayer.SendHapticImpulse(continuousHapticIntensity, continuousHapticDuration, 0.5f);
                //Debug.Log($"Continuous Haptic {hapticPlayer.name}: Full Intensity!");
            }
            isInsideInnerSphere = true;

            // Activate red sphere if not already active
            if (innerSphereVisual && !innerSphereVisual.activeSelf)
            {
                innerSphereVisual.SetActive(true);
            }
        }
        else
        {
            isInsideInnerSphere = false;

            // Deactivate red sphere if it was active
            if (innerSphereVisual && innerSphereVisual.activeSelf)
            {
                innerSphereVisual.SetActive(false);
            }

            if (closestDistance < outerSphereRadius && Time.time - lastHapticTime >= hapticInterval)
            {
                // Scaled vibration based on proximity
                float intensity = 1 - (closestDistance / outerSphereRadius);
                float duration = Mathf.Lerp(minHapticDuration, maxHapticDuration, intensity);
                hapticPlayer.SendHapticImpulse(intensity, duration, 0.5f);
                lastHapticTime = Time.time;
                //Debug.Log($"Haptic {hapticPlayer.name}: Intensity = {intensity:F2}, Duration = {duration:F2}");
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (leftController)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(leftController.position, outerSphereRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(leftController.position, innerSphereRadius);
        }

        if (rightController)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(rightController.position, outerSphereRadius);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(rightController.position, innerSphereRadius);
        }
    }
}
