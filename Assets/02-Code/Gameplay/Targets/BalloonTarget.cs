using UnityEngine;

public class BalloonTarget : MonoBehaviour, IRaycastHittable
{
  [SerializeField] private BalloonType targetType = BalloonType.Red;
  [SerializeField] private GameObject explosionEffect;
  [SerializeField] private AudioClip soundEffect;
  [SerializeField] private BalloonVisuals visuals;

  private bool wasHit;

  private void Awake()
  {
    if (visuals == null)
    {
      visuals = GetComponent<BalloonVisuals>();
    }

    ApplyVisuals();
  }

  public void Configure(BalloonType newType, float lifetime)
  {
    targetType = newType;
    wasHit = false;

    ApplyVisuals();

    CancelInvoke(nameof(SelfDestruct));
    Invoke(nameof(SelfDestruct), lifetime);
  }

  public void OnRaycastHit()
  {
    Hit();
  }

  private void OnCollisionEnter(Collision collision)
  {
    Hit();
  }

  public void Hit()
  {
    if (wasHit)
      return;

    wasHit = true;
    CancelInvoke(nameof(SelfDestruct));

    if (explosionEffect != null)
    {
      Instantiate(explosionEffect, transform.position, Quaternion.identity);
    }

    if (soundEffect != null)
    {
      AudioSource.PlayClipAtPoint(soundEffect, transform.position);
    }

    if (GameManager.Instance != null)
    {
      GameManager.Instance.RegisterHit(BalloonPoints.GetPoints(targetType));
    }

    Destroy(gameObject);
  }

  private void SelfDestruct()
  {
    Destroy(gameObject);
  }

  private void ApplyVisuals()
  {
    if (visuals != null)
    {
      visuals.Apply(targetType);
    }
  }
}