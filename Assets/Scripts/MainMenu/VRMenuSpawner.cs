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
    public List<string> menuSceneNames; // Corresponding scene names (e.g., "MainScene")
    
    [Header("Placement Settings")]
    public float menuDistance = 2f; // Distance from player
    public float menuHeight = 1.5f; // Height of buttons
    public float buttonWidth = 0.5f; // Width of each button
    public float rotationStep = 50f; // Angle between buttons

    private Dictionary<GameObject, string> buttonSceneMap = new Dictionary<GameObject, string>(); // Map buttons to scene names

    private void Start()
    {
        GenerateMenu();
    }

    // I wanted to have the buttons in a circle around the player so the buttons are boxes 
    // evenly spaced and facing the player 
    private void GenerateMenu()
    {
        int buttonCount = menuSceneNames.Count;
        float totalAngle = (buttonCount - 1) * rotationStep; // Spread buttons evenly

        for (int i = 0; i < buttonCount; i++)
        {
            float angle = -totalAngle / 2 + i * rotationStep; // Evenly space buttons
            Vector3 buttonPosition = playerHead.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * menuDistance;
            buttonPosition.y = menuHeight; // Set height

            // Spawn button (it's a box with a collider)
            GameObject button = Instantiate(buttonPrefab, buttonPosition, Quaternion.identity);
            button.gameObject.SetActive(true);
            button.transform.LookAt(new Vector3(playerHead.position.x, button.transform.position.y, playerHead.position.z)); // Face the player

            // Save button-scene mapping
            buttonSceneMap[button] = menuSceneNames[i];

            // Setup button component
            VRMenuButton menuButton = button.GetComponent<VRMenuButton>();
            if (menuButton)
            {
                menuButton.Initialize(menuSceneNames[i], this);
            }
        }
    }

    // when the button is hovered, and the player press A, it will call this function
    public void SelectButton(GameObject button)
    {
        if (buttonSceneMap.TryGetValue(button, out string sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
