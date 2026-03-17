using UnityEngine;

public class ExitGameButton : MonoBehaviour, IRaycastHittable
{
  public void OnRaycastHit()
  {
    Hit();
  }

  public void Hit()
  {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
  }
}