using UnityEngine;
using UnityEngine.InputSystem;

//everything is in the name
public class UseLidarOnClick : MonoBehaviour
{
    [SerializeField] private InputActionReference leftClickInput;   
    [SerializeField] private InputActionReference rightClickInput;   
    private InputAction clickAction;

    void Awake()
    {
        // Ajouter un listener pour détecter le bouton
        leftClickInput.action.performed += OnClickDetected;
        rightClickInput.action.performed += OnClickDetected;
    }

    private void OnClickDetected(InputAction.CallbackContext context)
    {
        LidarSystem.TriggerClickDetected();
    }
}