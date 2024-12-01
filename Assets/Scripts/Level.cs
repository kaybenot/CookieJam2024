using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Level
{
    public LevelSettings LevelSettings { get; }

    [CanBeNull] public Room CurrentRoom => currentRoom;

    public event Action<Enemy> OnEnemySpawned;
    public event Action<Enemy> OnEnemyDefeated;
    public event Action OnRoomSpawned;

    private Room currentRoom = null;
    
    public Level(LevelSettings settings)
    {
        LevelSettings = settings;
    }

    public void Initialize()
    {
        SpawnNextRoom();
    }

    public void Release()
    {
        if (currentRoom != null)
        {
            OnRoomSpawned = null;
            OnEnemySpawned = null;
            OnEnemyDefeated = null;
            Object.Destroy(currentRoom.gameObject);
        }
    }

    private void SpawnNextRoom()
    {
        if (currentRoom != null)
        {
            currentRoom.OnNextRoomRequest -= SpawnNextRoom;
            Release();
        }

        currentRoom = Object.Instantiate(GetNextRoom()).GetComponent<Room>();
        currentRoom.OnNextRoomRequest += SpawnNextRoom;
        currentRoom.OnEnemySpawned += (enemy) => OnEnemySpawned?.Invoke(enemy);
        currentRoom.OnEnemyDefeated += (enemy) => OnEnemyDefeated?.Invoke(enemy);
        currentRoom.Initialize(LevelSettings);
        OnRoomSpawned?.Invoke();
        
    }

    private GameObject GetNextRoom()
    {
        return LevelSettings.RoomPrefabs[Random.Range(0, LevelSettings.RoomPrefabs.Count)];
    }
}
