using System;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Player player;
    [SerializeField] private GameObject shapesController;

    private bool fighting = false;
    private float lastTimeEnemyAttacked = 0f;
    private Enemy currentEnemy = null;

    private void Awake()
    {
        levelManager.OnEnemySpawned += BeginFight;
        levelManager.OnEnemyDefeated += EndFight;
        
        shapesController.SetActive(false);
    }

    private void OnDestroy()
    {
        levelManager.OnEnemySpawned -= BeginFight;
        levelManager.OnEnemyDefeated -= EndFight;
    }

    private void BeginFight(Enemy enemy)
    {
        GameLog.Instance.Log("You have been attacked");
        lastTimeEnemyAttacked = Time.time;
        currentEnemy = enemy;

        enemy.OnAttack += OnEnemyAttack;

        fighting = true;
        shapesController.SetActive(true);
    }
    
    private void EndFight(Enemy enemy)
    {
        GameLog.Instance.Log("You have defeated an opponent");
        currentEnemy = null;

        fighting = false;
        shapesController.SetActive(false);
    }

    private void OnEnemyAttack(Sigil sigil)
    {
        // TODO: Attack logic, now only deals damage
        player.Damage(sigil.Damage);
    }

    private void Update()
    {
        if (!fighting || currentEnemy == null)
        {
            return;
        }

        if (lastTimeEnemyAttacked + currentEnemy.TimeToAttack > Time.time)
        {
            return;
        }

        lastTimeEnemyAttacked = Time.time;
        
        currentEnemy.Attack();
    }
}
