using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Systems")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private RoundTimer roundTimer;
    [SerializeField] private BalloonTypeProvider balloonTypeProvider;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI stateText;

    [Header("Flow")]
    [SerializeField] private float autoAdvanceDelay = 2f;
    [SerializeField] private float autoRestartDelay = 2f;

    public GameState CurrentState { get; private set; } = GameState.Idle;

    public bool RoundActive => CurrentState == GameState.Playing;

    public int CurrentLevelNumber => levelManager != null ? levelManager.CurrentLevelNumber : 1;

    public float CurrentSpawnInterval => levelManager != null ? levelManager.GetSpawnInterval() : 1f;

    public float CurrentBalloonSizeMultiplier => levelManager != null ? levelManager.GetBalloonSizeMultiplier() : 1f;

    public float CurrentBalloonLifetime => levelManager != null ? levelManager.GetBalloonLifetime() : 3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        if (scoreSystem != null)
        {
            scoreSystem.OnScoreChanged += HandleScoreChanged;
        }

        if (roundTimer != null)
        {
            roundTimer.OnTimeChanged += HandleTimeChanged;
            roundTimer.OnTimeExpired += HandleTimeExpired;
        }
    }

    private void OnDisable()
    {
        if (scoreSystem != null)
        {
            scoreSystem.OnScoreChanged -= HandleScoreChanged;
        }

        if (roundTimer != null)
        {
            roundTimer.OnTimeChanged -= HandleTimeChanged;
            roundTimer.OnTimeExpired -= HandleTimeExpired;
        }
    }

    private void Start()
    {
        InitializeIdleState();
    }

    public void BeginGame()
    {
        CancelInvoke(nameof(RestartGame));
        CancelInvoke(nameof(AdvanceToNextLevel));

        if (levelManager == null || scoreSystem == null || roundTimer == null)
        {
            Debug.LogError("[GameManager] Missing required system reference.");
            return;
        }

        levelManager.ResetToFirstLevel();
        StartCurrentLevel();
    }

    public void RegisterHit(int points)
    {
        if (!RoundActive || scoreSystem == null || levelManager == null)
            return;

        scoreSystem.AddScore(points);

        if (scoreSystem.CurrentScore >= levelManager.GetRequiredScore())
        {
            WinRound();
        }
    }

    public void RegisterMiss()
    {
        // Garde le hook pour plus tard si tu veux pénaliser les tirs ratés.
    }

    public bool TryConsumeBullet()
    {
        // Version de transition : tant que la manche est active, le tir est autorisé.
        // Plus tard, ça devra déléguer à AmmoSystem.
        return RoundActive;
    }

    public BalloonType GetRandomBalloonType()
    {
        if (balloonTypeProvider == null)
            return BalloonType.Red;

        return balloonTypeProvider.GetRandomBalloonType();
    }

    private void StartCurrentLevel()
    {
        if (levelManager == null || scoreSystem == null || roundTimer == null)
            return;

        LevelConfig currentLevel = levelManager.CurrentLevel;
        if (currentLevel == null)
        {
            Debug.LogError("[GameManager] No current level available.");
            return;
        }

        CurrentState = GameState.Playing;

        scoreSystem.ResetScore();
        roundTimer.StartTimer(currentLevel.roundDuration);

        RefreshStaticUI();
        SetStateText(string.Empty);
    }

    private void WinRound()
    {
        if (!RoundActive || levelManager == null || roundTimer == null)
            return;

        roundTimer.StopTimer();
        CancelInvoke(nameof(RestartGame));
        CancelInvoke(nameof(AdvanceToNextLevel));

        if (!levelManager.HasNextLevel())
        {
            CurrentState = GameState.Finished;
            SetStateText("Victoire finale");
            return;
        }

        CurrentState = GameState.Won;
        SetStateText("Niveau réussi");
        Invoke(nameof(AdvanceToNextLevel), autoAdvanceDelay);
    }

    private void LoseRound(string reason)
    {
        if (!RoundActive || roundTimer == null || levelManager == null)
            return;

        CurrentState = GameState.Lost;
        roundTimer.StopTimer();

        CancelInvoke(nameof(AdvanceToNextLevel));
        SetStateText("Défaite : " + reason);
        Invoke(nameof(RestartGame), autoRestartDelay);
    }

    private void AdvanceToNextLevel()
    {
        if (levelManager == null)
            return;

        levelManager.GoToNextLevel();
        StartCurrentLevel();
    }

    private void RestartGame()
    {
        if (levelManager == null)
            return;

        levelManager.ResetToFirstLevel();
        StartCurrentLevel();
    }

    private void InitializeIdleState()
    {
        CurrentState = GameState.Idle;

        if (levelManager != null)
        {
            levelManager.ResetToFirstLevel();
        }

        if (scoreSystem != null)
        {
            scoreSystem.ResetScore();
        }

        RefreshStaticUI();

        if (roundTimer != null && levelManager != null)
        {
            HandleTimeChanged(levelManager.GetRoundDuration());
        }

        SetStateText("Tire sur Start pour jouer");
    }

    private void RefreshStaticUI()
    {
        if (levelManager == null)
            return;

        if (levelText != null)
        {
            levelText.text = "Niveau : " + levelManager.CurrentLevelNumber;
        }

        if (objectiveText != null)
        {
            objectiveText.text = "Objectif : " + levelManager.GetRequiredScore();
        }
    }

    private void HandleScoreChanged(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + score;
        }
    }

    private void HandleTimeChanged(float timeRemaining)
    {
        if (timerText != null)
        {
            timerText.text = "Temps : " + Mathf.CeilToInt(timeRemaining);
        }
    }

    private void HandleTimeExpired()
    {
        LoseRound("Temps écoulé");
    }

    private void SetStateText(string message)
    {
        if (stateText != null)
        {
            stateText.text = message;
        }
    }
}