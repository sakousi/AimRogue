using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRaycast : MonoBehaviour
{
    //distance maximale du rayon
    public float range = 100f;

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            Debug.Log("Mouse Position: " + mousePos);
            ShootRay(mousePos);
        }
    }

    void ShootRay(Vector3 mousePos)
    {

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            hit.collider.GetComponent<life>()?.OnCollisionEnter(new Collision());
        }
        else
        {
            Debug.Log("Raycast: rien touché (vérifie colliders/layers et distance).");
        }
    }
}