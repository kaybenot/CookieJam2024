using System;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    private void Awake()
    {
        levelManager.OnEnemySpawned += BeginFight;
        levelManager.OnEnemyDefeated += EndFight;
    }

    private void OnDestroy()
    {
        levelManager.OnEnemySpawned -= BeginFight;
        levelManager.OnEnemyDefeated -= EndFight;
    }

    private void BeginFight(Enemy enemy)
    {
        GameLog.Instance.Log("You have been attacked");
    }
    
    private void EndFight(Enemy enemy)
    {
        GameLog.Instance.Log("You have defeated an opponent");
    }
}
