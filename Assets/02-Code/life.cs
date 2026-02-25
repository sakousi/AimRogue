using UnityEngine;

public class life : MonoBehaviour
{

    public GameObject explosionEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
    }
}
