using System;
using UnityEngine;

public class RoundTimer : MonoBehaviour
{
  public float TimeRemaining { get; private set; }
  public bool IsRunning { get; private set; }

  public event Action<float> OnTimeChanged;
  public event Action OnTimeExpired;

  public void StartTimer(float duration)
  {
    TimeRemaining = duration;
    IsRunning = true;
    OnTimeChanged?.Invoke(TimeRemaining);
  }

  public void StopTimer()
  {
    IsRunning = false;
  }

  private void Update()
  {
    if (!IsRunning)
      return;

    TimeRemaining -= Time.deltaTime;

    if (TimeRemaining <= 0f)
    {
      TimeRemaining = 0f;
      IsRunning = false;
      OnTimeChanged?.Invoke(TimeRemaining);
      OnTimeExpired?.Invoke();
      return;
    }

    OnTimeChanged?.Invoke(TimeRemaining);
  }
}