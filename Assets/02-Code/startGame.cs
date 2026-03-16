using UnityEngine;

public class startGame : MonoBehaviour
{
    public void Hit()
    {
        Debug.Log("[startGame] Start object hit");

        hidden menuHidden = GetComponentInParent<hidden>();
        if (menuHidden != null)
        {
            menuHidden.Hide();
        }

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.BeginGame();
        }
        else
        {
            Debug.LogWarning("[startGame] No ScoreManager instance found");
        }
    }
}
