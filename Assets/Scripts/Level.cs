using UnityEngine;

public class Level
{
    public LevelSettings LevelSettings { get; }

    private Room currentRoom = null;
    
    public Level(LevelSettings settings)
    {
        LevelSettings = settings;
        
        SpawnRoom(settings);
    }

    public void Release()
    {
        if (currentRoom != null)
        {
            // var spawner = Object.FindFirstObjectByType<EnemySpawner>();
            // if (spawner != null)
            // {
            //     Object.Destroy(spawner.gameObject);
            // }
            
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
        currentRoom.Initialize(settings);
    }

    private GameObject GetNextRoom()
    {
        return LevelSettings.RoomPrefabs[Random.Range(0, LevelSettings.RoomPrefabs.Count)];
    }
}
