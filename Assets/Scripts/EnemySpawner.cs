using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public Action<Enemy> OnEnemySpawned { get; set; }
    public Action<Enemy> OnEnemyDefeated { get; set; }

    private Enemy currentEnemy = null;

    public void Spawn(LevelSettings settings)
    {
        currentEnemy = Instantiate(GetEnemy(settings), transform.position, Quaternion.identity).GetComponent<Enemy>();
        currentEnemy.OnDeath += (enemy) =>
        {
            OnEnemyDefeated?.Invoke(enemy);
            currentEnemy = null;
        };
        OnEnemySpawned?.Invoke(currentEnemy);
    }

    private GameObject GetEnemy(LevelSettings settings)
    {
        return settings.EnemyPrefabs[Random.Range(0, settings.EnemyPrefabs.Count)];
    }
}
