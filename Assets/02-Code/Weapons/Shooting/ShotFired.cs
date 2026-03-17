using UnityEngine;

public class ShotFired : MonoBehaviour
{
  [SerializeField] private ParticleSystem muzzleFlash;
  [SerializeField] private AudioSource audioSource;
  [SerializeField] private AudioClip shotSound;

  public void Play()
  {
    if (muzzleFlash != null)
    {
      muzzleFlash.Play();
    }

    if (audioSource != null && shotSound != null)
    {
      audioSource.PlayOneShot(shotSound);
    }
  }
}