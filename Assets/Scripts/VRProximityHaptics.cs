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
    [SerializeField] private float hapticInterval = 0.3f;
    [SerializeField] private float continuousHapticIntensity = 1.0f;
    [SerializeField] private float continuousHapticDuration = 0.1f;

    [Header("Visual Feedback")]
    public GameObject leftInnerSphereVisual;
    public GameObject rightInnerSphereVisual;
    public Material defaultRedMaterial;
    public Material blueMaterial;

    private float lastLeftHapticTime;
    private float lastRightHapticTime;

    private bool leftInsideInnerSphere = false;
    private bool rightInsideInnerSphere = false;

    private int environmentLayer = 8;
    private int lidarTriggerLayer = 7;
    private int lidar2TriggerLayer = 9;

    private void Update()
    {
        ProcessProximity(
            leftController,
            leftHapticPlayer,
            leftInnerSphereVisual,
            ref lastLeftHapticTime,
            ref leftInsideInnerSphere
        );

        ProcessProximity(
            rightController,
            rightHapticPlayer,
            rightInnerSphereVisual,
            ref lastRightHapticTime,
            ref rightInsideInnerSphere
        );
    }

    private void ProcessProximity(
        Transform controller,
        HapticImpulsePlayer hapticPlayer,
        GameObject innerSphereVisual,
        ref float lastHapticTime,
        ref bool isInsideInnerSphere
    )
    {
        Collider[] hitColliders = Physics.OverlapSphere(controller.position, outerSphereRadius);

        float closestDistance = float.MaxValue;
        bool isTouchingInnerSphere = false;
        bool showBlue = false;

        foreach (var col in hitColliders)
        {
            if (col.gameObject == controller.gameObject)
                continue;

            int colLayer = col.gameObject.layer;
            Vector3 closest = col.ClosestPointOnBounds(controller.position);
            float distance = Vector3.Distance(controller.position, closest);

            // Évite les fausses distances nulles
            if (distance < 0.01f)
                distance = Vector3.Distance(controller.position, col.bounds.center);

            // 💢 Ignore anything not in environment or lidar layer
            if (colLayer != environmentLayer && colLayer != lidarTriggerLayer && colLayer != lidar2TriggerLayer)
                continue;

            // 🔵 Check if it's a Lidar-Trigger object (layer 7)
            if (colLayer == lidarTriggerLayer || colLayer == lidar2TriggerLayer)
            {
                // Show blue sphere if Lidar layer is detected
                showBlue = true;
            }

            // 🔴 Haptics only apply to Environment layer (layer 8)
            if (colLayer == environmentLayer)
            {
                if (distance <= innerSphereRadius)
                {
                    isTouchingInnerSphere = true;
                    break; // No need to continue
                }

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                }
            }
        }

        // 🔥 Inner Sphere Feedback
        if (isTouchingInnerSphere)
        {
            if (!isInsideInnerSphere)
            {
                hapticPlayer.SendHapticImpulse(continuousHapticIntensity, continuousHapticDuration, 0.5f);
            }
            isInsideInnerSphere = true;

            if (innerSphereVisual && !innerSphereVisual.activeSelf)
            {
                innerSphereVisual.SetActive(true);
            }

            SetSphereMaterial(innerSphereVisual, showBlue ? blueMaterial : defaultRedMaterial);
        }
        else
        {
            isInsideInnerSphere = false;

            // 🟢 Show sphere in blue if Lidar layer is detected (even if not in inner radius)
            if (showBlue)
            {
                if (innerSphereVisual && !innerSphereVisual.activeSelf)
                {
                    innerSphereVisual.SetActive(true);
                }
                SetSphereMaterial(innerSphereVisual, blueMaterial);
            }
            else
            {
                if (innerSphereVisual && innerSphereVisual.activeSelf)
                {
                    innerSphereVisual.SetActive(false);
                }
            }

            // 💢 Normal proximity haptics
            if (closestDistance < outerSphereRadius && Time.time - lastHapticTime >= hapticInterval)
            {
                float intensity = 1 - (closestDistance / outerSphereRadius);
                float duration = Mathf.Lerp(minHapticDuration, maxHapticDuration, intensity);
                hapticPlayer.SendHapticImpulse(intensity, duration, 0.5f);
                lastHapticTime = Time.time;
            }
        }
    }

    private void SetSphereMaterial(GameObject sphere, Material mat)
    {
        if (sphere.TryGetComponent<Renderer>(out Renderer rend))
        {
            rend.material = mat;
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
