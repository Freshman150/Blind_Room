using UnityEngine;
using UnityEngine.InputSystem;

public class TST : MonoBehaviour
{
    [SerializeField] private InputActionReference leftClickInput;   
    private InputAction clickAction;

    void Awake()
    {
        // Ajouter un listener pour détecter le bouton
        leftClickInput.action.performed += OnClickDetected;
    }

    private void OnClickDetected(InputAction.CallbackContext context)
    {
        Debug.Log("Click détecté ! Activation du Lidar.");
        LidarSystem.TriggerClickDetected();
    }
}