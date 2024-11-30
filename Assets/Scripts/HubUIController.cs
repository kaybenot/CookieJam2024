using UnityEngine;

public class HubUIController : MonoBehaviour
{
    [SerializeField]
    private HubController hubController;
    [SerializeField]
    private CanvasGroup hubUiCanvasGroup;
    
    private void Awake()
    {
        hubUiCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        hubController.OnLevelStarted += HubController_OnLevelStarted;
        hubController.OnLevelEnded += HubController_OnLevelEnded;
    }

    private void HubController_OnLevelStarted(LevelSettings level)
    {
        SetUIActive(false);
    }

    private void HubController_OnLevelEnded(LevelSettings level)
    {
        SetUIActive(true);
    }

    private void SetUIActive(bool active)
    {
        hubUiCanvasGroup.alpha = active ? 1 : 0;
        hubUiCanvasGroup.interactable = active;
        hubUiCanvasGroup.blocksRaycasts = active;
    }

    private void OnDisable()
    {
        hubController.OnLevelStarted -= HubController_OnLevelStarted;
        hubController.OnLevelEnded -= HubController_OnLevelEnded;
    }
}
