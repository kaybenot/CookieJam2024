using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask interactableLayer;

    private void Start()
    {
        InputManager.Instance.OnLeftClick += TryInteract;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnLeftClick -= TryInteract;
    }

    private void TryInteract(float screenX, float screenY)
    {
        if (enabled == false)
            return;

        if (Camera.main == null)
        {
            return;
        }

        var cam = Camera.main;
        var ray = cam.ScreenPointToRay(new Vector3(screenX, screenY));
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 30f);

        if (Physics.Raycast(ray, out var hit, float.PositiveInfinity, interactableLayer))
        {
            var target = hit.transform;
            while (target != null)
            {
                if (target.TryGetComponent<IInteractable>(out var interactable))
                {
                    interactable.Interact();
                    break;
                }
                
                target = target.parent;
            }
        }
    }
}
