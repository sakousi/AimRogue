using UnityEngine;

public class life : MonoBehaviour
{
    public GameObject explosionEffect;
    public AudioClip soundEffect;

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        ScoreManager.instance.AddPoint(10);
    }
}
