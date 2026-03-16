using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(WeaponReload))]
[RequireComponent(typeof(WeaponCameraRaycast))]
[RequireComponent(typeof(ShotFired))]
public class WeaponFire : MonoBehaviour
{
    [Header("Fire")]
    public float fireRate = 0.5f;
    private float nextTimeToFire = 0f;
    private bool isFiring = false;

    [Header("References")]
    public WeaponCameraRaycast raycast;
    public ShotFired shotFired;
    public WeaponReload reload;
    public PlayerCameraRecoil playerCameraRecoil;

    [Header("Recoil")]
    public float recoilKickBack = 0.08f;
    public float recoilRotationX = 8f;
    public float recoilRotationY = 2f;
    public float recoilReturnSpeed = 8f;
    public float recoilSnappiness = 16f;

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    private Vector3 currentRecoilPosition;
    private Vector3 targetRecoilPosition;

    private Vector3 currentRecoilRotation;
    private Vector3 targetRecoilRotation;

    void Start()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;

            if (reload != null && reload.isReloading)
            {
                return;
            }

            isFiring = true;

            Shoot();
        }

        if(isFiring)
        {
            HandleRecoil();
        }
    }

    void Shoot()
    {
        Debug.Log("Weapon Fired!");
        shotFired.FireShot();
        raycast.Shoot();
        ShootRecoil();
    }

    void ShootRecoil()
    {
        // Recul en position : l'arme part légèrement en arrière
        targetRecoilPosition += new Vector3(0f, 0f, -recoilKickBack);

        // Recul en rotation : l'arme monte + léger décalage horizontal aléatoire
        targetRecoilRotation += new Vector3(
            -recoilRotationX,
            UnityEngine.Random.Range(-recoilRotationY, recoilRotationY),
            0f
        );

        if (playerCameraRecoil != null)
        {
            playerCameraRecoil.ApplyRecoil(2f, 0.8f);
        }
    }

    void HandleRecoil()
    {
        if (targetRecoilPosition.magnitude < 0.01f && targetRecoilRotation.magnitude < 0.01f)
        {
            isFiring = false;
        }

        // Les cibles reviennent progressivement vers zéro
        targetRecoilPosition = Vector3.Lerp(
            targetRecoilPosition,
            Vector3.zero,
            recoilReturnSpeed * Time.deltaTime
        );

        targetRecoilRotation = Vector3.Lerp(
            targetRecoilRotation,
            Vector3.zero,
            recoilReturnSpeed * Time.deltaTime
        );

        // Le recoil courant suit la cible avec un effet plus nerveux
        currentRecoilPosition = Vector3.Lerp(
            currentRecoilPosition,
            targetRecoilPosition,
            recoilSnappiness * Time.deltaTime
        );

        currentRecoilRotation = Vector3.Lerp(
            currentRecoilRotation,
            targetRecoilRotation,
            recoilSnappiness * Time.deltaTime
        );

        // Application sur l'arme
        transform.localPosition = initialLocalPosition + currentRecoilPosition;
        transform.localRotation = initialLocalRotation * Quaternion.Euler(currentRecoilRotation);
    }
}