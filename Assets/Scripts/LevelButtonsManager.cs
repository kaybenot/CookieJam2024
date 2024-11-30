using UnityEngine;

public class LevelButtonsManager : MonoBehaviour
{
    [SerializeField]
    private HubController hubController;
    [SerializeField]
    private Transform buttonsContainer;
    [SerializeField]
    private LevelButton buttonPrototype;

    private void Start()
    {
        int levelsCount = hubController.UnlockedLevelsCount;
        for (int i = 0; i < levelsCount; i++)
        {
            SpawnButton(i);
        }
    }

    private void SpawnButton(int levelIndex)
    {
        var button = Instantiate(buttonPrototype, buttonsContainer);
        button.Init(levelIndex);
        button.OnClicked += Button_OnClicked;
    }

    private void Button_OnClicked(LevelButton button)
    {
        hubController.StartLevel(button.LevelIndex);
    }

    private void OnEnable()
    {
        hubController.OnLevelUnlocked += HubController_OnLevelUnlocked;
    }

    private void HubController_OnLevelUnlocked()
    {
        int levelNumber = hubController.UnlockedLevelsCount - 1;
        SpawnButton(levelNumber);
    }

    private void OnDisable()
    {
        hubController.OnLevelUnlocked -= HubController_OnLevelUnlocked;
    }
}
