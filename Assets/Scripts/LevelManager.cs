using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LevelSettings editorLevel;
    
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
