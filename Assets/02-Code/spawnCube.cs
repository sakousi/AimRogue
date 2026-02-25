using UnityEngine;

public class spawnCube : MonoBehaviour

{
    public GameObject cubePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnPrefab", 1f, 1f);
        // stop spawning after 10 seconds
        Invoke("StopSpawning", 30f);
    }

    public void SpawnPrefab()
    {
        GameObject cube = Instantiate((cubePrefab));
        cube.transform.position = new Vector3(Random.Range(-10, 10), 50, Random.Range(-10, 10));
    }

    public void StopSpawning()
    {
        CancelInvoke("SpawnPrefab");
    }
}
