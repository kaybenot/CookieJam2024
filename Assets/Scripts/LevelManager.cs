using System;
using JetBrains.Annotations;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LevelSettings editorLevel;
    [SerializeField] private FightManager fightManager;
    [SerializeField] private Interactor interactor;

    public event Action OnRoomSpawned;
    public event Action OnLevelStarted;

    [CanBeNull] public Level CurrentLevel => currentLevel;
    
    private Level currentLevel = null;

    [ContextMenu("Run editorLevel")]
    private void RunEditorLevel()
    {
        RunLevel(editorLevel);
    }

    [ContextMenu("Kill enemies")]
    private void KillEnemies()
    {
        foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            enemy.Damage(100000);
        }
    }
    
    public void RunLevel(LevelSettings settings)
    {
        ReleaseLevel();
        currentLevel = new Level(settings);
        currentLevel.OnEnemySpawned += CurrentLevel_OnEnemySpawned;
        currentLevel.OnEnemyDefeated += CurrentLevel_OnEnemyDefeated;
        currentLevel.OnRoomSpawned += () => OnRoomSpawned?.Invoke();
        currentLevel.Initialize();
        OnLevelStarted?.Invoke();
    }

    private void CurrentLevel_OnEnemySpawned(Enemy enemy)
    {
        interactor.enabled = false;
        fightManager.BeginFight(enemy);
    }

    private void CurrentLevel_OnEnemyDefeated(Enemy enemy)
    {
        interactor.enabled = true;
        fightManager.EndFight();
    }

    public void ReleaseLevel()
    {
        if (currentLevel == null)
        {
            return;
        }

        currentLevel.OnEnemySpawned -= CurrentLevel_OnEnemySpawned;
        currentLevel.OnEnemyDefeated -= CurrentLevel_OnEnemyDefeated;
        currentLevel.Release();
        currentLevel = null;
        KillEnemies();
    }
}
