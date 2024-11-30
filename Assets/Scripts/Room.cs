using System;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    public Action OnNextRoomRequest { get; set; }

    public void Initialize(LevelSettings settings)
    {
        foreach (var spawner in FindObjectsByType<EnemySpawner>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if (!spawner.enabled)
            {
                continue;
            }
            
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
    }

    public void GoToNextRoom()
    {
        OnNextRoomRequest?.Invoke();
    }
}
