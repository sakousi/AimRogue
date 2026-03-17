using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
  public int CurrentScore { get; private set; }

  public event Action<int> OnScoreChanged;

  public void ResetScore()
  {
    CurrentScore = 0;
    OnScoreChanged?.Invoke(CurrentScore);
  }

  public void AddScore(int points)
  {
    CurrentScore += points;
    OnScoreChanged?.Invoke(CurrentScore);
  }
}