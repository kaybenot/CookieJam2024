using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputEventProvider : MonoBehaviour
{
    public event System.Action OnPressed;
    public event System.Action OnReleased;

    [SerializeField]
    private InputActionReference pressInputActionReference;
    private InputAction pressInputAction;

    [SerializeField]
    private Vector2 mouseDelta;

    [SerializeField]
    private float deltaThreshold = 2;

    [SerializeField]
    private bool isDragging;
    public bool IsDragging => isDragging;

    private void OnEnable()
    {
        pressInputAction = pressInputActionReference.action.Clone();
        pressInputAction.Enable();
        pressInputAction.canceled += Action_canceled;
    }

    private void Update()
    {
        mouseDelta = Mouse.current.delta.value;
        if (isDragging == false && pressInputAction.IsInProgress() && mouseDelta.sqrMagnitude > deltaThreshold * deltaThreshold)
        {
            StartDragging();       
        }
        
        else if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            OnReleased?.Invoke();
        }
    }

    private void Action_canceled(InputAction.CallbackContext obj) => StartDragging();

    private void StartDragging()
    {
        if (isDragging)
            return;

        isDragging = true;
        OnPressed?.Invoke();
    }

    private void OnDisable()
    {
        pressInputAction.canceled -= Action_canceled;
        pressInputAction.Disable();
    }
}
