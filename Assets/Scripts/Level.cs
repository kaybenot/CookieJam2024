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
        SpawnRoom(LevelSettings);
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

    private void SpawnRoom(LevelSettings settings)
    {
        if (currentRoom != null)
        {
            currentRoom.OnNextRoomRequest -= () => SpawnRoom(settings);
            Release();
        }

        currentRoom = Object.Instantiate(GetNextRoom()).GetComponent<Room>();
        currentRoom.OnNextRoomRequest += () => SpawnRoom(settings);
        currentRoom.OnEnemySpawned += (enemy) => OnEnemySpawned?.Invoke(enemy);
        currentRoom.OnEnemyDefeated += (enemy) => OnEnemyDefeated?.Invoke(enemy);
        currentRoom.Initialize(settings);
        OnRoomSpawned?.Invoke();
        
    }

    private GameObject GetNextRoom()
    {
        return LevelSettings.RoomPrefabs[Random.Range(0, LevelSettings.RoomPrefabs.Count)];
    }
}
