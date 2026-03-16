using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Hit()
    {
        Debug.Log("[ExitGame] Exit object hit, quitting game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
