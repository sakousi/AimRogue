using System;
using UnityEngine;

public class WeaponCameraRaycast : MonoBehaviour
{
    public float range = 100f;
    public Camera fpsCam;

    public void Shoot()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, range))
        {
            hit.collider.GetComponent<life>()?.OnCollisionEnter(new Collision());
        }
    }
}