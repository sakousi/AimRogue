using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Game/Level Database")]
public class LevelDatabase : ScriptableObject
{
    public LevelConfig[] levels;
}