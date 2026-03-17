using UnityEngine;

public class BalloonVisuals : MonoBehaviour
{
  [SerializeField] private bool autoApplyColor = true;
  private Renderer cachedRenderer;

  private void Awake()
  {
    cachedRenderer = GetComponentInChildren<Renderer>();
  }

  public void Apply(BalloonType type)
  {
    if (!autoApplyColor || cachedRenderer == null)
      return;

    cachedRenderer.material.color = GetColor(type);
  }

  private Color GetColor(BalloonType type)
  {
    switch (type)
    {
      case BalloonType.Red: return Color.red;
      case BalloonType.Blue: return Color.blue;
      case BalloonType.Yellow: return Color.yellow;
      case BalloonType.Violet: return new Color(0.6f, 0.2f, 1f);
      case BalloonType.Black: return Color.black;
      default: return Color.white;
    }
  }
}