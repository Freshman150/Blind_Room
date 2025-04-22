using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class VRMenuSpawner : MonoBehaviour
{
    [Header("Menu Settings")]
    public Transform playerHead; // VR headset position
    public GameObject buttonPrefab; // Prefab for menu buttons (with colliders)
    
    [Header("Menu Options")]
    public List<string> menuDisplayNames; // Displayed names (e.g., "Start Game")
    public List<string> menuSceneNames; // Corresponding scene names (e.g., "MainScene")
    
    [Header("Placement Settings")]
    public float menuDistance = 2f; // Distance from player
    public float menuHeight = 1.5f; // Height of buttons
    public float buttonWidth = 0.5f; // Width of each button
    public float rotationStep = 50f; // Angle between buttons

    private Dictionary<GameObject, string> buttonSceneMap = new Dictionary<GameObject, string>();

    private void Start()
    {
        GenerateMenu();
    }

    private void GenerateMenu()
    {
        if (menuDisplayNames.Count != menuSceneNames.Count)
        {
            Debug.LogError("Menu Display Names and Scene Names lists must be the same length!");
            return;
        }

        int buttonCount = menuDisplayNames.Count;
        float totalAngle = (buttonCount - 1) * rotationStep; // Spread buttons evenly

        for (int i = 0; i < buttonCount; i++)
        {
            float angle = -totalAngle / 2 + i * rotationStep; // Evenly space buttons
            Vector3 buttonPosition = playerHead.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * menuDistance;
            buttonPosition.y = menuHeight; // Set height

            // Spawn button
            GameObject button = Instantiate(buttonPrefab, buttonPosition, Quaternion.identity);
            button.gameObject.SetActive(true);
            button.transform.LookAt(new Vector3(playerHead.position.x, button.transform.position.y, playerHead.position.z)); // Face the player
            button.name = menuDisplayNames[i]; // Set display name for debugging

            // Save button-scene mapping
            buttonSceneMap[button] = menuSceneNames[i];

            // Setup button component
            VRMenuButton menuButton = button.GetComponent<VRMenuButton>();
            if (menuButton)
            {
                menuButton.Initialize(menuDisplayNames[i], menuSceneNames[i], this);
            }
        }
    }

    public void SelectButton(GameObject button)
    {
        if (buttonSceneMap.TryGetValue(button, out string sceneName))
        {
            IEnumerator ReloadXR()
            {
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
                yield return null;  // Attendre 1 frame
                yield return null;  // Attendre 1 autre frame
                XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
                XRGeneralSettings.Instance.Manager.StartSubsystems();
            }
            //StartCoroutine(ReloadXR());
            Debug.Log($"Loading Scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
    }
}
