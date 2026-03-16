using UnityEngine;

public class life : MonoBehaviour
{
    public enum TargetType
    {
        Red,
        Blue,
        Yellow,
        Violet,
        Black
    }

    public TargetType targetType = TargetType.Red;
    public GameObject explosionEffect;
    public AudioClip soundEffect;
    public bool autoApplyColor = true;

    private Renderer cachedRenderer;
    private bool wasHit;

    void Awake()
    {
        cachedRenderer = GetComponentInChildren<Renderer>();
        Debug.Log("[life] Awake on " + gameObject.name + " rendererFound=" + (cachedRenderer != null));
        ApplyTypeVisuals();
    }

    public void Configure(TargetType newType, float lifetime)
    {
        targetType = newType;
        ApplyTypeVisuals();
        CancelInvoke(nameof(SelfDestruct));
        Invoke(nameof(SelfDestruct), lifetime);
        Debug.Log("[life] Configure name=" + gameObject.name + " type=" + targetType + " lifetime=" + lifetime);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Hit();
    }

    public void Hit()
    {
        if (wasHit)
        {
            Debug.Log("[life] Hit ignored, already hit on " + gameObject.name);
            return;
        }

        wasHit = true;
        CancelInvoke(nameof(SelfDestruct));
        Debug.Log("[life] Hit on " + gameObject.name + " type=" + targetType);

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        if (soundEffect != null)
        {
            AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        }

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.RegisterHit(GetPointsForType(targetType));
        }

        Destroy(gameObject);
    }

    private void SelfDestruct()
    {
        Debug.Log("[life] SelfDestruct on " + gameObject.name);
        Destroy(gameObject);
    }

    private void ApplyTypeVisuals()
    {
        if (!autoApplyColor || cachedRenderer == null)
        {
            Debug.Log("[life] ApplyTypeVisuals skipped autoApplyColor=" + autoApplyColor + " rendererFound=" + (cachedRenderer != null));
            return;
        }

        cachedRenderer.material.color = GetColorForType(targetType);
        Debug.Log("[life] ApplyTypeVisuals color applied type=" + targetType);
    }

    public static int GetPointsForType(TargetType type)
    {
        switch (type)
        {
            case TargetType.Red: return 100;
            case TargetType.Blue: return 200;
            case TargetType.Yellow: return 300;
            case TargetType.Violet: return 500;
            case TargetType.Black: return -200;
            default: return 0;
        }
    }

    private Color GetColorForType(TargetType type)
    {
        switch (type)
        {
            case TargetType.Red: return Color.red;
            case TargetType.Blue: return Color.blue;
            case TargetType.Yellow: return Color.yellow;
            case TargetType.Violet: return new Color(0.6f, 0.2f, 1f);
            case TargetType.Black: return Color.black;
            default: return Color.white;
        }
    }
}
