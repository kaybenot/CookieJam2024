using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour, IInteractable
{
    public UnityEvent OnDoorClicked;
    public bool Locked { get; set; } = true;
    
    public void Interact()
    {
        if (Locked)
        {
            GameLog.Instance.Log("Door is locked.");
            return;
        }
        OnDoorClicked?.Invoke();
    }
}
