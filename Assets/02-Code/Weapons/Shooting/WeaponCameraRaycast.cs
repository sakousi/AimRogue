using UnityEngine;

public class WeaponCameraRaycast : MonoBehaviour
{
    [SerializeField] private Camera raycastCamera;
    [SerializeField] private float range = 100f;

    public bool ShootRay()
    {
        Camera cam = raycastCamera != null ? raycastCamera : Camera.main;
        if (cam == null)
            return false;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            IRaycastHittable hittable = hit.collider.GetComponent<IRaycastHittable>();
            if (hittable != null)
            {
                hittable.OnRaycastHit();
                return true;
            }
        }

        return false;
    }
}