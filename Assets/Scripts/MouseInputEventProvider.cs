using UnityEngine;

public class MouseInputEventProvider : MonoBehaviour
{
    public event System.Action OnPressed;
    public event System.Action OnReleased;

    [SerializeField]
    private bool isPressed;
    public bool IsPressed => isPressed;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;
            OnPressed?.Invoke();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
            OnReleased?.Invoke();
        }
    }
}
