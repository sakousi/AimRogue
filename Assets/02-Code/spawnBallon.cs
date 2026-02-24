using UnityEngine;

public class spawnBallon : MonoBehaviour
{
    public GameObject ballonPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnPrefab", 1f, 1f);
        // stop spawning after 10 seconds
        Invoke("StopSpawning", 30f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnPrefab()
    {
        GameObject ballon = Instantiate((ballonPrefab));
        
        ballon.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(2, 10), 1);
    }

    public void StopSpawning()
    {
        CancelInvoke("SpawnPrefab");
    }
}
