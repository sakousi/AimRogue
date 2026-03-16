using UnityEngine;
using UnityEngine.InputSystem;

public class ShotFired : MonoBehaviour
{
    public AudioClip shotSound;
    public GameObject shotParticleEffect;

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) //left mouse button
        {
            FireShot();
        }
    }

    public void FireShot()
    {
        AudioSource.PlayClipAtPoint(shotSound, transform.position);
        if (shotParticleEffect != null)
        {   
            Instantiate(shotParticleEffect, transform.position, Quaternion.identity);
        }
    }

}
