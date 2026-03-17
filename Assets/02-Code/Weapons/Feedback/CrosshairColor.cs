using UnityEngine;
using UnityEngine.UI;

public class CrosshairColor : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private Image crosshairImage;
  [SerializeField] private Camera targetCamera;

  [Header("Settings")]
  [SerializeField] private float range = 100f;
  [SerializeField] private Color defaultColor = Color.white;
  [SerializeField] private Color hittableColor = Color.red;

  private void Update()
  {
    UpdateCrosshairColor();
  }

  private void UpdateCrosshairColor()
  {
    if (crosshairImage == null)
      return;

    Camera cam = targetCamera != null ? targetCamera : Camera.main;
    if (cam == null)
    {
      crosshairImage.color = defaultColor;
      return;
    }

    Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

    if (Physics.Raycast(ray, out RaycastHit hit, range))
    {
      IRaycastHittable hittable = hit.collider.GetComponent<IRaycastHittable>();

      crosshairImage.color = hittable != null ? hittableColor : defaultColor;
      return;
    }

    crosshairImage.color = defaultColor;
  }
}