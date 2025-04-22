using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class VRMenuRaycaster : MonoBehaviour
{
    [Header("References")]
    public Transform playerHead; // Headset position
    public LayerMask menuLayer; // Menu object layer
    public InputActionReference selectButton; // Controller button for selection
    public HapticImpulsePlayer hapticPlayer;

    private GameObject lastHoveredButton;

    private void Start()
    {
        selectButton.action.Enable();
        selectButton.action.performed += _ => ConfirmSelection();
    }

    private void Update()
    {
        Ray ray = new Ray(playerHead.position, playerHead.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, menuLayer))
        {
            GameObject hitButton = hit.collider.gameObject;

            if (hitButton != lastHoveredButton)
            {
                lastHoveredButton = hitButton;
                OnButtonFocused(hitButton);
            }
        }
        else
        {
            lastHoveredButton = null;
        }
    }

    private void OnButtonFocused(GameObject button)
    {
        hapticPlayer.SendHapticImpulse(0.5f, 0.1f, 0.5f);
        
        // Get button name for narration
        VRMenuButton menuButton = button.GetComponent<VRMenuButton>();
        if (menuButton)
        {
            DummyNarrator.Instance.ReadOption(menuButton.GetDisplayName());
            StartCoroutine(DummyNarrator.Instance.PlaySpeech(DummyNarrator.Instance._hoverSpeech));
        }

        Debug.Log($"Looking at: {button.name}");
    }

    private void ConfirmSelection()
    {
        if (lastHoveredButton != null)
        {
            VRMenuButton menuButton = lastHoveredButton.GetComponent<VRMenuButton>();
            if (menuButton)
            {
                StartCoroutine(DummyNarrator.Instance.PlaySpeech(DummyNarrator.Instance._onClickSpeech));
                menuButton.Select();
            }
        }
    }

    private void OnDisable()
    {
        selectButton.action.performed -= _ => ConfirmSelection();
        selectButton.action.Disable();
    }
}