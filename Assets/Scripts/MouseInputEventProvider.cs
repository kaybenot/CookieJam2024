using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputEventProvider : MonoBehaviour
{
    public event System.Action OnPressed;
    public event System.Action OnReleased;

    [SerializeField]
    private InputActionReference pressInputActionReference;
    private InputActionReference pressInputAction;

    [SerializeField]
    private Vector2 mouseDelta;

    [SerializeField]
    private float deltaThreshold = 2;

    [SerializeField]
    private bool isDragging;
    public bool IsDragging => isDragging;

    private void OnEnable()
    {
        pressInputAction = Instantiate(pressInputActionReference);
        pressInputAction.action.Enable();
        pressInputAction.action.canceled += Action_canceled;
    }

    private void Update()
    {
        mouseDelta = Mouse.current.delta.value;
        if (isDragging == false && pressInputAction.action.IsInProgress() && mouseDelta.sqrMagnitude > deltaThreshold * deltaThreshold)
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
        pressInputAction.action.canceled -= Action_canceled;
        pressInputAction.action.Disable();
    }
}
