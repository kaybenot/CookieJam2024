using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Scriptable Objects/LevelSettings")]
public class LevelSettings : ScriptableObject
{
    public List<GameObject> RoomPrefabs;
    public List<GameObject> EnemyPrefabs;
}
