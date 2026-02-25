using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI scoreText;  
    private int currentScore = 0;

    void Awake()
    {

        instance = this;
    }

    public void AddPoint(int points)
    {
        currentScore += points;
        scoreText.text = "Score : " + currentScore;
    }
}