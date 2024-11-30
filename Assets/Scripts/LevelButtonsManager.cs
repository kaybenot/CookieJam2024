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
            SpawnButton(hubController.Levels.GetLevel(i));
        }
    }

    private void SpawnButton(LevelSettings level)
    {
        var button = Instantiate(buttonPrototype, buttonsContainer);
        button.Init(level);
    }

    private void OnEnable()
    {
        hubController.OnLevelUnlocked += HubController_OnLevelUnlocked;
    }

    private void HubController_OnLevelUnlocked()
    {
        int levelNumber = hubController.UnlockedLevelsCount - 1;
        var level = hubController.Levels.GetLevel(levelNumber);
        SpawnButton(level);
    }

    private void OnDisable()
    {
        hubController.OnLevelUnlocked -= HubController_OnLevelUnlocked;
    }
}
