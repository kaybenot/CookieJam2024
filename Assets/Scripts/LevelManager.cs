using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LevelSettings editorLevel;
    
    public Action<Enemy> OnEnemySpawned { get; set; }
    public Action<Enemy> OnEnemyDefeated { get; set; }
    public Action OnRoomSpawned { get; set; }
    public Action OnLevelStarted { get; set; }

    [CanBeNull] public Level CurrentLevel => currentLevel;
    
    private Level currentLevel = null;
    public Level CurrentLevel => currentLevel;

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
        currentLevel.OnEnemySpawned += (enemy) => OnEnemySpawned?.Invoke(enemy);
        currentLevel.OnEnemyDefeated += (enemy) => OnEnemyDefeated?.Invoke(enemy);
        currentLevel.OnRoomSpawned += () => OnRoomSpawned?.Invoke();
        currentLevel.Initialize();
        OnLevelStarted?.Invoke();
    }

    public void ReleaseLevel()
    {
        if (currentLevel == null)
        {
            return;
        }

        currentLevel.Release();
        currentLevel = null;
        KillEnemies();
    }
}
