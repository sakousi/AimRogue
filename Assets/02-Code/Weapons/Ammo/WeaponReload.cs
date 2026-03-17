using System;
using System.Collections;
using UnityEngine;

public class WeaponReload : MonoBehaviour
{
  [Header("Ammo")]
  [SerializeField] private WeaponConfig weaponConfig;

  [Header("Audio")]
  [SerializeField] private AudioSource ReloadAudioSource;
  [SerializeField] private AudioClip ReloadAudioClip;

  public int CurrentAmmo { get; private set; }
  public int ReserveAmmo { get; private set; }
  public bool IsReloading { get; private set; }

  public bool HasInfiniteMagazine => weaponConfig != null && weaponConfig.magazineSize < 0;
  public bool HasInfiniteReserve => weaponConfig != null && weaponConfig.startingReserveAmmo < 0;
  public bool HasInfiniteAmmo => HasInfiniteMagazine || HasInfiniteReserve;

  public event Action<int, int> OnAmmoChanged;

  private void Awake()
  {
    if (weaponConfig == null)
    {
      Debug.LogError("[WeaponReload] WeaponConfig is missing.");
      return;
    }

    CurrentAmmo = HasInfiniteMagazine ? -1 : weaponConfig.magazineSize;
    ReserveAmmo = HasInfiniteReserve ? -1 : weaponConfig.startingReserveAmmo;

    NotifyAmmoChanged();
  }

  public bool CanShoot()
  {
    if (IsReloading)
      return false;

    if (HasInfiniteMagazine)
      return true;

    return CurrentAmmo > 0;
  }

  public bool CanReload()
  {
    if (IsReloading || weaponConfig == null)
      return false;

    if (HasInfiniteMagazine)
      return false;

    if (HasInfiniteReserve)
      return CurrentAmmo < weaponConfig.magazineSize;

    return CurrentAmmo < weaponConfig.magazineSize && ReserveAmmo > 0;
  }

  public bool TryConsumeBullet()
  {
    if (!CanShoot())
      return false;

    if (!HasInfiniteMagazine)
    {
      CurrentAmmo--;
      NotifyAmmoChanged();
    }

    return true;
  }

  public void StartReload(MonoBehaviour owner)
  {
    if (!CanReload())
      return;

    owner.StartCoroutine(ReloadCoroutine());
  }

  private IEnumerator ReloadCoroutine()
  {
    IsReloading = true;

    if (ReloadAudioSource != null && ReloadAudioClip != null)
    {
      ReloadAudioSource.PlayOneShot(ReloadAudioClip);
    }

    yield return new WaitForSeconds(weaponConfig.reloadDuration);

    if (HasInfiniteReserve)
    {
      CurrentAmmo = weaponConfig.magazineSize;
    }
    else
    {
      int missingAmmo = weaponConfig.magazineSize - CurrentAmmo;
      int ammoToLoad = Mathf.Min(missingAmmo, ReserveAmmo);

      CurrentAmmo += ammoToLoad;
      ReserveAmmo -= ammoToLoad;
    }

    IsReloading = false;
    NotifyAmmoChanged();
  }

  private void NotifyAmmoChanged()
  {
    OnAmmoChanged?.Invoke(CurrentAmmo, ReserveAmmo);
  }
}