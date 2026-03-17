using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelConfig
    {
        public int requiredScore = 1000;
        public float roundDuration = 30f;
        public float spawnInterval = 1f;
        public float balloonSizeMultiplier = 1f;
        public float balloonLifetime = 3f;
    }

    public static ScoreManager instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI stateText;

    [Header("Gameplay")]
    public LevelConfig[] levels;
    public float autoAdvanceDelay = 2f;
    public float autoRestartDelay = 2f;

    private int currentLevelIndex;
    private int currentScore;
    private float timeRemaining;
    private bool roundActive;

    public bool RoundActive => roundActive;
    public int CurrentLevelNumber => currentLevelIndex + 1;
    public float CurrentSpawnInterval => CurrentLevel.spawnInterval;
    public float CurrentBalloonSizeMultiplier => CurrentLevel.balloonSizeMultiplier;
    public float CurrentBalloonLifetime => CurrentLevel.balloonLifetime;

    private LevelConfig CurrentLevel => levels[Mathf.Clamp(currentLevelIndex, 0, levels.Length - 1)];

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (levels == null || levels.Length == 0)
        {
            levels = CreateDefaultLevels();
        }
    }

    void Start()
    {
        InitializeIdleState();
    }

    void Update()
    {
        if (!roundActive)
        {
            return;
        }

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            RefreshUI();
            LoseRound("Temps ecoule");
            return;
        }

        RefreshUI();
    }

    public void BeginGame()
    {
        CancelInvoke(nameof(RestartGame));
        CancelInvoke(nameof(AdvanceToNextLevel));
        StartLevel(0);
    }

    public void RegisterHit(int points)
    {
        if (!roundActive)
        {
            return;
        }

        currentScore += points;
        RefreshUI();

        if (currentScore >= CurrentLevel.requiredScore)
        {
            WinRound();
        }
    }

    public void RegisterMiss()
    {
        // Garde la methode pour compatibilite avec le raycast.
    }

    public bool TryConsumeBullet()
    {
        // Version simplifiee : plus de gestion des balles.
        return roundActive;
    }

    public life.TargetType GetRandomBalloonType()
    {
        int roll = Random.Range(0, 100);

        if (roll < 35) return life.TargetType.Red;
        if (roll < 60) return life.TargetType.Blue;
        if (roll < 80) return life.TargetType.Yellow;
        if (roll < 92) return life.TargetType.Violet;

        return life.TargetType.Black;
    }

    private void WinRound()
    {
        if (!roundActive)
        {
            return;
        }

        roundActive = false;
        CancelInvoke(nameof(RestartGame));
        CancelInvoke(nameof(AdvanceToNextLevel));

        if (currentLevelIndex >= levels.Length - 1)
        {
            SetStateText("Victoire finale");
            return;
        }

        SetStateText("Niveau reussi");
        Invoke(nameof(AdvanceToNextLevel), autoAdvanceDelay);
    }

    private void LoseRound(string reason)
    {
        if (!roundActive)
        {
            return;
        }

        roundActive = false;
        CancelInvoke(nameof(AdvanceToNextLevel));
        SetStateText("Defaite : " + reason);
        Invoke(nameof(RestartGame), autoRestartDelay);
    }

    private void AdvanceToNextLevel()
    {
        StartLevel(currentLevelIndex + 1);
    }

    private void RestartGame()
    {
        StartLevel(0);
    }

    private void StartLevel(int levelIndex)
    {
        currentLevelIndex = Mathf.Clamp(levelIndex, 0, levels.Length - 1);
        currentScore = 0;
        timeRemaining = CurrentLevel.roundDuration;
        roundActive = true;
        SetStateText(string.Empty);
        RefreshUI();
    }

    private void InitializeIdleState()
    {
        currentLevelIndex = 0;
        currentScore = 0;
        timeRemaining = levels != null && levels.Length > 0 ? levels[0].roundDuration : 0f;
        roundActive = false;
        SetStateText("Tire sur Start pour jouer");
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + currentScore;
        }

        if (timerText != null)
        {
            timerText.text = "Temps : " + Mathf.CeilToInt(timeRemaining);
        }

        if (levelText != null)
        {
            levelText.text = "Niveau : " + CurrentLevelNumber;
        }

        if (objectiveText != null)
        {
            objectiveText.text = "Objectif : " + CurrentLevel.requiredScore;
        }
    }

    private void SetStateText(string message)
    {
        if (stateText != null)
        {
            stateText.text = message;
        }
    }

    private LevelConfig[] CreateDefaultLevels()
    {
        return new[]
        {
            new LevelConfig { requiredScore = 1000, roundDuration = 30f, spawnInterval = 1f, balloonSizeMultiplier = 1f, balloonLifetime = 3f },
            new LevelConfig { requiredScore = 2000, roundDuration = 28f, spawnInterval = 0.85f, balloonSizeMultiplier = 0.9f, balloonLifetime = 2.6f },
            new LevelConfig { requiredScore = 3500, roundDuration = 26f, spawnInterval = 0.7f, balloonSizeMultiplier = 0.8f, balloonLifetime = 2.2f },
            new LevelConfig { requiredScore = 5000, roundDuration = 24f, spawnInterval = 0.55f, balloonSizeMultiplier = 0.7f, balloonLifetime = 1.8f }
        };
    }
}
