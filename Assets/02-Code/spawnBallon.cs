using UnityEngine;

public class spawnBallon : MonoBehaviour
{
    public GameObject ballonPrefab;
    public float sizeBallon;
    public float ymin;
    public float ymax;
    public float xmin;
    public float xmax;
    public float zmin;
    public float zmax;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnPrefab", 1f, 1f);
        // stop spawning after 10 seconds
        Invoke("StopSpawning", 30f);
    }

    public void SpawnPrefab()
    {
        GameObject ballon = Instantiate(ballonPrefab);
        // position aléatoire dans la zone du parent, ne pas exeder les limites du parent
        ballon.transform.localPosition = new Vector3(Random.Range(xmin, xmax), Random.Range(ymin, ymax), Random.Range(zmin, zmax));
        ballon.transform.localScale = Vector3.one * sizeBallon;
    }

    public void StopSpawning()
    {
        CancelInvoke("SpawnPrefab");
    }
}
