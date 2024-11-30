using UnityEngine;
using UnityEngine.InputSystem;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private InputActionReference inputAction;

    private void OnEnable()
    {
        var action = inputAction.action;
        action.Enable();
        action.performed += Action_performed;
        action.started += Action_started;
        action.canceled += Action_canceled;
    
    }

    private void Action_canceled(InputAction.CallbackContext obj)
    {
        Debug.Log("Cancelled");
    }

    private void Action_started(InputAction.CallbackContext obj)
    {
        Debug.Log("Started");
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("Performed");
    }

    private void OnDisable()
    {
        inputAction.action.Disable();
    }
}
