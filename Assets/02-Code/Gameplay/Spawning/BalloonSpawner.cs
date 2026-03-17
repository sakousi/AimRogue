using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
  [System.Serializable]
  public class BalloonPrefabEntry
  {
    public BalloonType type;
    public GameObject prefab;
  }

  [SerializeField] private BalloonPrefabEntry[] prefabs;
  [SerializeField] private GameObject fallbackPrefab;
  [SerializeField] private float baseBalloonSize = 1f;
  [SerializeField] private float fallbackSpawnInterval = 1f;
  [SerializeField] private float fallbackLifetime = 3f;

  [Header("Spawn Bounds")]
  [SerializeField] private float xmin;
  [SerializeField] private float xmax;
  [SerializeField] private float ymin;
  [SerializeField] private float ymax;
  [SerializeField] private float zmin;
  [SerializeField] private float zmax;

  private void Start()
  {
    Debug.Log("[BalloonSpawner] Start");
    ScheduleNextSpawn();
  }

  private void ScheduleNextSpawn()
  {
    float delay = fallbackSpawnInterval;

    if (GameManager.Instance != null)
    {
      delay = GameManager.Instance.CurrentSpawnInterval;
    }

    Debug.Log("[BalloonSpawner] ScheduleNextSpawn delay=" + delay);
    CancelInvoke(nameof(SpawnBalloon));
    Invoke(nameof(SpawnBalloon), delay);
  }

  private void SpawnBalloon()
  {
    Debug.Log("[BalloonSpawner] SpawnBalloon called");

    if (fallbackPrefab == null && (prefabs == null || prefabs.Length == 0))
    {
      Debug.LogError("[BalloonSpawner] No prefab assigned.");
      ScheduleNextSpawn();
      return;
    }

    if (GameManager.Instance != null)
    {
      Debug.Log("[BalloonSpawner] RoundActive=" + GameManager.Instance.RoundActive);

      if (!GameManager.Instance.RoundActive)
      {
        Debug.LogWarning("[BalloonSpawner] Round inactive, skipping spawn.");
        ScheduleNextSpawn();
        return;
      }
    }

    BalloonType type = GameManager.Instance != null
        ? GameManager.Instance.GetRandomBalloonType()
        : BalloonType.Red;

    Debug.Log("[BalloonSpawner] Selected type=" + type);

    GameObject prefab = GetPrefabForType(type);
    if (prefab == null)
    {
      Debug.LogError("[BalloonSpawner] Prefab is null for type " + type);
      ScheduleNextSpawn();
      return;
    }

    Vector3 spawnPosition = transform.position + new Vector3(
        Random.Range(xmin, xmax),
        Random.Range(ymin, ymax),
        Random.Range(zmin, zmax)
    );

    Debug.Log("[BalloonSpawner] Spawning at " + spawnPosition);

    GameObject balloon = Instantiate(prefab, spawnPosition, Quaternion.identity);

    float sizeMultiplier = GameManager.Instance != null
        ? GameManager.Instance.CurrentBalloonSizeMultiplier
        : 1f;

    balloon.transform.localScale = Vector3.one * baseBalloonSize * sizeMultiplier;

    BalloonTarget target = balloon.GetComponent<BalloonTarget>();
    if (target != null)
    {
      float lifetime = GameManager.Instance != null
          ? GameManager.Instance.CurrentBalloonLifetime
          : fallbackLifetime;

      target.Configure(type, lifetime);
      Debug.Log("[BalloonSpawner] Balloon configured lifetime=" + lifetime);
    }
    else
    {
      Debug.LogWarning("[BalloonSpawner] Spawned prefab has no BalloonTarget component.");
    }

    ScheduleNextSpawn();
  }

  private GameObject GetPrefabForType(BalloonType type)
  {
    if (prefabs != null)
    {
      foreach (var entry in prefabs)
      {
        if (entry.type == type && entry.prefab != null)
          return entry.prefab;
      }
    }

    return fallbackPrefab;
  }
}