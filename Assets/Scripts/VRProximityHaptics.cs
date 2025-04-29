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

    private int environmentLayer = 8; //used for the environment layer 
    private int lidarTriggerLayer = 7; //used for the first object layer
    private int lidar2TriggerLayer = 9; //used for the second object layer

    private void Update()
    {
        // process proximity for left controllers
        ProcessProximity(
            leftController,
            leftHapticPlayer,
            leftInnerSphereVisual,
            ref lastLeftHapticTime,
            ref leftInsideInnerSphere
        );
        
        // process proximity for right controllers
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

        // get the closest collider and check if it's in the inner sphere
        foreach (var col in hitColliders)
        {
            if (col.gameObject == controller.gameObject)
                continue;
            
            //get the closest point on the collider
            int colLayer = col.gameObject.layer;
            Vector3 closest = col.ClosestPointOnBounds(controller.position);
            float distance = Vector3.Distance(controller.position, closest);

            //  evade false distance (there are some fake distance values that are 0 sometimes) 
            if (distance < 0.01f)
                distance = Vector3.Distance(controller.position, col.bounds.center);

            // 💢 Ignore anything not in environment or lidar layer
            if (colLayer != environmentLayer && colLayer != lidarTriggerLayer && colLayer != lidar2TriggerLayer)
                continue;

            // 🔵 Check if it's an object (meaning you can interact with it) (layer 7 or 9)
            if (colLayer == lidarTriggerLayer || colLayer == lidar2TriggerLayer)
            {
                // Show blue sphere if object layer is detected
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

            // 🟢 Show sphere in blue if object layer is detected (even if not in inner radius)
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

    //chance color of the sphere depending if it"s an object or not
    private void SetSphereMaterial(GameObject sphere, Material mat)
    {
        if (sphere.TryGetComponent<Renderer>(out Renderer rend))
        {
            rend.material = mat;
        }
    }
}
