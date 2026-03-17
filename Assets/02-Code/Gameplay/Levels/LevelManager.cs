using UnityEngine;

public class LevelManager : MonoBehaviour
{
  [Header("Data")]
  [SerializeField] private LevelDatabase levelDatabase;

  public int CurrentLevelIndex { get; private set; }

  public int CurrentLevelNumber => CurrentLevelIndex + 1;

  public int LevelCount
  {
    get
    {
      if (levelDatabase == null || levelDatabase.levels == null)
        return 0;

      return levelDatabase.levels.Length;
    }
  }

  public LevelConfig CurrentLevel
  {
    get
    {
      if (levelDatabase == null || levelDatabase.levels == null || levelDatabase.levels.Length == 0)
        return null;

      return levelDatabase.levels[Mathf.Clamp(CurrentLevelIndex, 0, levelDatabase.levels.Length - 1)];
    }
  }

  private void Awake()
  {
    if (levelDatabase == null)
    {
      Debug.LogError("[LevelManager] LevelDatabase is NULL. Assign it in inspector.");
    }
  }

  // =========================
  // CONTROL
  // =========================

  public void ResetToFirstLevel()
  {
    CurrentLevelIndex = 0;
  }

  public void SetLevel(int index)
  {
    if (LevelCount == 0)
      return;

    CurrentLevelIndex = Mathf.Clamp(index, 0, LevelCount - 1);
  }

  public bool HasNextLevel()
  {
    return LevelCount > 0 && CurrentLevelIndex < LevelCount - 1;
  }

  public void GoToNextLevel()
  {
    if (!HasNextLevel())
      return;

    CurrentLevelIndex++;
  }

  // =========================
  // SAFE GETTERS (optionnel)
  // =========================

  public float GetSpawnInterval()
  {
    return CurrentLevel != null ? CurrentLevel.spawnInterval : 1f;
  }

  public float GetBalloonSizeMultiplier()
  {
    return CurrentLevel != null ? CurrentLevel.balloonSizeMultiplier : 1f;
  }

  public float GetBalloonLifetime()
  {
    return CurrentLevel != null ? CurrentLevel.balloonLifetime : 3f;
  }

  public float GetRoundDuration()
  {
    return CurrentLevel != null ? CurrentLevel.roundDuration : 30f;
  }

  public int GetRequiredScore()
  {
    return CurrentLevel != null ? CurrentLevel.requiredScore : 0;
  }
}