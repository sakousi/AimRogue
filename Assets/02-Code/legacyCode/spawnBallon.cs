using UnityEngine;

public class spawnBallon : MonoBehaviour
{
    public GameObject ballonPrefab;
    public GameObject redBalloonPrefab;
    public GameObject blueBalloonPrefab;
    public GameObject yellowBalloonPrefab;
    public GameObject violetBalloonPrefab;
    public GameObject blackBalloonPrefab;
    public float sizeBallon = 1f;
    public float ymin;
    public float ymax;
    public float xmin;
    public float xmax;
    public float zmin;
    public float zmax;
    public float fallbackSpawnInterval = 1f;
    public float fallbackLifetime = 3f;

    void Start()
    {
        Debug.Log("[spawnBallon] Start on " + gameObject.name);
        ScheduleNextSpawn();
    }

    void ScheduleNextSpawn()
    {
        float spawnDelay = fallbackSpawnInterval;

        if (ScoreManager.instance != null)
        {
            spawnDelay = ScoreManager.instance.CurrentSpawnInterval;
        }

        Debug.Log("[spawnBallon] ScheduleNextSpawn delay=" + spawnDelay + " scoreManager=" + (ScoreManager.instance != null));

        CancelInvoke(nameof(SpawnPrefab));
        Invoke(nameof(SpawnPrefab), spawnDelay);
    }

    public void SpawnPrefab()
    {
        Debug.Log("[spawnBallon] SpawnPrefab called");

        if (ballonPrefab == null)
        {
            Debug.LogError("[spawnBallon] ballonPrefab is NULL");
            ScheduleNextSpawn();
            return;
        }

        if (ScoreManager.instance != null && !ScoreManager.instance.RoundActive)
        {
            Debug.Log("[spawnBallon] Round inactive, skipping spawn");
            ScheduleNextSpawn();
            return;
        }

        life.TargetType targetType = ScoreManager.instance != null
            ? ScoreManager.instance.GetRandomBalloonType()
            : life.TargetType.Red;

        GameObject prefabToSpawn = GetPrefabForType(targetType);
        GameObject ballon = Instantiate(prefabToSpawn);
        ballon.transform.position = transform.position + new Vector3(
            Random.Range(xmin, xmax),
            Random.Range(ymin, ymax),
            Random.Range(zmin, zmax));

        Debug.Log("[spawnBallon] Spawned " + ballon.name + " type=" + targetType + " at world position " + ballon.transform.position);

        float levelSizeMultiplier = ScoreManager.instance != null ? ScoreManager.instance.CurrentBalloonSizeMultiplier : 1f;
        ballon.transform.localScale = Vector3.one * sizeBallon * levelSizeMultiplier;

        Debug.Log("[spawnBallon] Applied scale=" + ballon.transform.localScale + " baseSize=" + sizeBallon + " levelMultiplier=" + levelSizeMultiplier);

        life balloonLife = ballon.GetComponent<life>();
        if (balloonLife != null)
        {
            float lifetime = ScoreManager.instance != null ? ScoreManager.instance.CurrentBalloonLifetime : fallbackLifetime;
            balloonLife.Configure(targetType, lifetime);
            Debug.Log("[spawnBallon] Configured balloon type=" + targetType + " lifetime=" + lifetime);
        }
        else
        {
            Debug.LogWarning("[spawnBallon] Spawned prefab has no life component");
        }

        ScheduleNextSpawn();
    }

    GameObject GetPrefabForType(life.TargetType targetType)
    {
        switch (targetType)
        {
            case life.TargetType.Red:
                if (redBalloonPrefab != null) return redBalloonPrefab;
                break;
            case life.TargetType.Blue:
                if (blueBalloonPrefab != null) return blueBalloonPrefab;
                break;
            case life.TargetType.Yellow:
                if (yellowBalloonPrefab != null) return yellowBalloonPrefab;
                break;
            case life.TargetType.Violet:
                if (violetBalloonPrefab != null) return violetBalloonPrefab;
                break;
            case life.TargetType.Black:
                if (blackBalloonPrefab != null) return blackBalloonPrefab;
                break;
        }

        Debug.LogWarning("[spawnBallon] Specific prefab missing for " + targetType + ", using fallback ballonPrefab");
        return ballonPrefab;
    }
}
