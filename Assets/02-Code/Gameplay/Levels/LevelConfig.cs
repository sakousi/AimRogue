using System;
using UnityEngine;

[Serializable]
public class LevelConfig
{
  public int requiredScore = 1000;
  public float roundDuration = 30f;
  public float spawnInterval = 1f;
  public float balloonSizeMultiplier = 1f;
  public float balloonLifetime = 3f;
}