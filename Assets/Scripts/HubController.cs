using UnityEngine;

public class HubController : MonoBehaviour
{
    public event System.Action OnLevelUnlocked;
    public event System.Action<LevelSettings> OnLevelStarted;
    public event System.Action<LevelSettings> OnLevelEnded;

    [SerializeField]
    private LevelsSettings levels;
    public LevelsSettings Levels => levels;

    [SerializeField]
    private LevelManager levelManager;  

    [SerializeField]
    private int unlockedLevels = 1; 
    public int UnlockedLevelsCount => levels ? Mathf.Min(unlockedLevels, levels.LevelsCount) : 0;

    private void Awake()
    {
        unlockedLevels = 1;
    }

    public void StartLevel(int levelNumber)
    {
        if (levelNumber >= unlockedLevels)
        {
            return;
        }

        var level = levels.GetLevel(levelNumber);
        levelManager.RunLevel(level);
        OnLevelStarted?.Invoke(level);
    }

    public void EndLevel()
    {
        var level = levelManager.CurrentLevel;
        levelManager.ReleaseLevel();
        OnLevelEnded?.Invoke(level.LevelSettings);
    }
}
