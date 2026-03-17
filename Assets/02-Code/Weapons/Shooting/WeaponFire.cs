using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponFire : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private WeaponConfig weaponConfig;
  [SerializeField] private WeaponReload weaponReload;
  [SerializeField] private WeaponCameraRaycast weaponCameraRaycast;
  [SerializeField] private ShotFired shotFired;
  [SerializeField] private PlayerCameraRecoil playerCameraRecoil;

  [Header("Weapon Visual Recoil")]
  [SerializeField] private Transform weaponVisual;
  [SerializeField] private float visualReturnSpeed = 8f;
  [SerializeField] private float visualSnappiness = 12f;

  private float nextTimeToFire;

  private bool isBursting;
  private int burstShotsRemaining;

  private Vector3 visualInitialPosition;
  private Quaternion visualInitialRotation;
  private Vector3 visualTargetPosition;
  private Vector3 visualTargetRotation;

  private void Start()
  {
    if (weaponVisual != null)
    {
      visualInitialPosition = weaponVisual.localPosition;
      visualInitialRotation = weaponVisual.localRotation;
    }
  }

  private void Update()
  {
    HandleFireInput();
    UpdateWeaponVisualRecoil();
  }

  private void HandleFireInput()
  {
    if (weaponConfig == null || weaponReload == null)
      return;

    if (weaponReload.IsReloading)
      return;

    switch (weaponConfig.fireMode)
    {
      case WeaponFireMode.ShotByShot:
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
          TryFire();
        }
        break;

      case WeaponFireMode.FullAuto:
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
          TryFire();
        }
        break;

      case WeaponFireMode.Burst:
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && !isBursting)
        {
          StartBurst();
        }
        break;
    }

    if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
    {
      weaponReload.StartReload(this);
    }
  }

  private void TryFire()
  {
    if (Time.time < nextTimeToFire)
      return;

    nextTimeToFire = Time.time + (1f / weaponConfig.fireRate);

    if (!weaponReload.TryConsumeBullet())
      return;

    shotFired?.Play();
    weaponCameraRaycast?.ShootRay();
    ApplyWeaponVisualRecoil();

    if (playerCameraRecoil != null)
    {
      playerCameraRecoil.ApplyRecoil(
          weaponConfig.cameraRecoilX,
          weaponConfig.cameraRecoilY
      );
    }
  }

  private void StartBurst()
  {
    burstShotsRemaining = weaponConfig.burstCount;
    StartCoroutine(BurstCoroutine());
  }

  private IEnumerator BurstCoroutine()
  {
    isBursting = true;

    while (burstShotsRemaining > 0)
    {
      if (weaponReload == null || weaponReload.IsReloading)
        break;

      TryFire();
      burstShotsRemaining--;

      yield return new WaitForSeconds(1f / weaponConfig.fireRate);
    }

    isBursting = false;
  }

  private void ApplyWeaponVisualRecoil()
  {
    if (weaponVisual == null || weaponConfig == null)
      return;

    visualTargetPosition += new Vector3(0f, 0f, -weaponConfig.weaponKickback);
    visualTargetRotation += new Vector3(-weaponConfig.weaponRotationKick, 0f, 0f);
  }

  private void UpdateWeaponVisualRecoil()
  {
    if (weaponVisual == null)
      return;

    visualTargetPosition = Vector3.Lerp(visualTargetPosition, Vector3.zero, visualReturnSpeed * Time.deltaTime);
    visualTargetRotation = Vector3.Lerp(visualTargetRotation, Vector3.zero, visualReturnSpeed * Time.deltaTime);

    Vector3 currentPosition = Vector3.Lerp(
        weaponVisual.localPosition,
        visualInitialPosition + visualTargetPosition,
        visualSnappiness * Time.deltaTime
    );

    Quaternion currentRotation = Quaternion.Lerp(
        weaponVisual.localRotation,
        visualInitialRotation * Quaternion.Euler(visualTargetRotation),
        visualSnappiness * Time.deltaTime
    );

    weaponVisual.localPosition = currentPosition;
    weaponVisual.localRotation = currentRotation;
  }
}