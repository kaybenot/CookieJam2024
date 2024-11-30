using UnityEngine;

[CreateAssetMenu]
public class LevelsSettings : ScriptableObject
{
    [SerializeField]
    private LevelSettings[] orderedLevels;
    public int LevelsCount => orderedLevels.Length;

    public LevelSettings GetLevel(int levelIndex) => orderedLevels[levelIndex];
}
