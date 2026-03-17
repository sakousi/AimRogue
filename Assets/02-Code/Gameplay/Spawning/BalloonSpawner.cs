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
    ScheduleNextSpawn();
  }

  private void ScheduleNextSpawn()
  {
    float delay = fallbackSpawnInterval;

    if (GameManager.Instance != null)
    {
      delay = GameManager.Instance.CurrentSpawnInterval;
    }

    CancelInvoke(nameof(SpawnBalloon));
    Invoke(nameof(SpawnBalloon), delay);
  }

  private void SpawnBalloon()
  {
    if (fallbackPrefab == null && (prefabs == null || prefabs.Length == 0))
    {
      ScheduleNextSpawn();
      return;
    }

    if (GameManager.Instance != null && !GameManager.Instance.RoundActive)
    {
      ScheduleNextSpawn();
      return;
    }

    BalloonType type = GameManager.Instance != null
        ? GameManager.Instance.GetRandomBalloonType()
        : BalloonType.Red;

    GameObject prefab = GetPrefabForType(type);
    if (prefab == null)
    {
      ScheduleNextSpawn();
      return;
    }

    GameObject balloon = Instantiate(prefab);

    balloon.transform.position = transform.position + new Vector3(
        Random.Range(xmin, xmax),
        Random.Range(ymin, ymax),
        Random.Range(zmin, zmax)
    );

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