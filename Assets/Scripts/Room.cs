using System;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    public Action OnNextRoomRequest { get; set; }
    public Action OnDoorsUnlocked { get; set; }
    public Action<Enemy> OnEnemySpawned { get; set; }
    public Action<Enemy> OnEnemyDefeated { get; set; }
    public EnemySpawner Spawner { get; private set; }

    public void Initialize(LevelSettings settings)
    {
        foreach (var spawner in FindObjectsByType<EnemySpawner>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if (!spawner.enabled)
            {
                continue;
            }

            Spawner = spawner;
            spawner.OnEnemyDefeated += enemy => OnEnemyDefeated?.Invoke(enemy); 
            spawner.OnEnemySpawned += enemy => OnEnemySpawned?.Invoke(enemy); 
            spawner.Spawn(settings);
            spawner.OnEnemyDefeated += UnlockDoors;
        }
    }

    private void OnDestroy()
    {
        foreach (var spawner in FindObjectsByType<EnemySpawner>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            spawner.OnEnemyDefeated -= UnlockDoors;
        }
    }

    public void UnlockDoors(Enemy enemy)
    {
        foreach (var door in FindObjectsByType<Door>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            door.Locked = false;
        }
        
        OnDoorsUnlocked?.Invoke();
    }

    // attached to button
    public void GoToNextRoom()
    {
        OnNextRoomRequest?.Invoke();
    }
}
