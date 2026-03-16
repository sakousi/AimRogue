using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FireMode
{
    FullAuto,
    Burst,
    ShotByShot
}

[RequireComponent(typeof(WeaponReload))]
[RequireComponent(typeof(WeaponCameraRaycast))]
[RequireComponent(typeof(ShotFired))]
public class WeaponFire : MonoBehaviour
{
    [Header("Fire")]
    public float fireRate = 0.5f;
    public FireMode fireMode = FireMode.ShotByShot;
    public int burstCount = 3;

    private float nextTimeToFire = 0f;
    private bool isFiring = false;
    private bool isBursting = false;
    private int burstShotsRemaining = 0;

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
        if (reload != null && reload.isReloading)
        {
            HandleRecoilIfNeeded();
            return;
        }

        switch (fireMode)
        {
            case FireMode.ShotByShot:
                HandleShotByShot();
                break;

            case FireMode.FullAuto:
                HandleFullAuto();
                break;

            case FireMode.Burst:
                HandleBurstInput();
                break;
        }

        HandleBurstFire();
        HandleRecoilIfNeeded();
    }

    void HandleShotByShot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            isFiring = true;
            Shoot();
        }
    }

    void HandleFullAuto()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            isFiring = true;
            Shoot();
        }
    }

    void HandleBurstInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isBursting)
        {
            isBursting = true;
            burstShotsRemaining = burstCount;
        }
    }

    void HandleBurstFire()
    {
        if (!isBursting)
        {
            return;
        }

        if (Time.time >= nextTimeToFire)
        {
            if (burstShotsRemaining > 0)
            {
                nextTimeToFire = Time.time + fireRate;
                isFiring = true;
                Shoot();
                burstShotsRemaining--;
            }

            if (burstShotsRemaining <= 0)
            {
                isBursting = false;
            }
        }
    }

    void HandleRecoilIfNeeded()
    {
        if (isFiring)
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

        if (targetRecoilPosition.magnitude < 0.01f &&
            targetRecoilRotation.magnitude < 0.01f &&
            currentRecoilPosition.magnitude < 0.01f &&
            currentRecoilRotation.magnitude < 0.01f)
        {
            isFiring = false;
            transform.localPosition = initialLocalPosition;
            transform.localRotation = initialLocalRotation;
        }
    }
}