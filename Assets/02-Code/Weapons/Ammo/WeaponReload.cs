using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class WeaponReload : MonoBehaviour
{
  [Header("Ammo")]
  [SerializeField] private WeaponConfig weaponConfig;

  public int CurrentAmmo { get; private set; }
  public int ReserveAmmo { get; private set; }
  public bool IsReloading { get; private set; }
  public AudioSource reloadSource;
  public AudioClip reloadSound;


  public event Action<int, int> OnAmmoChanged;

  private void Awake()
  {
    if (weaponConfig == null)
    {
      Debug.LogError("[WeaponReload] WeaponConfig is missing.");
      return;
    }

    CurrentAmmo = weaponConfig.magazineSize;
    ReserveAmmo = weaponConfig.startingReserveAmmo;
    NotifyAmmoChanged();
  }

  public bool CanShoot()
  {
    return !IsReloading && CurrentAmmo > 0;
  }

  public bool CanReload()
  {
    return !IsReloading
        && CurrentAmmo < weaponConfig.magazineSize
        && ReserveAmmo > 0;
  }

  public bool TryConsumeBullet()
  {
    if (!CanShoot())
      return false;

    CurrentAmmo--;
    NotifyAmmoChanged();
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

    if (reloadSource != null && reloadSound != null)
    {
      reloadSource.PlayOneShot(reloadSound);
    }

    yield return new WaitForSeconds(weaponConfig.reloadDuration);

    int missingAmmo = weaponConfig.magazineSize - CurrentAmmo;
    int ammoToLoad = Mathf.Min(missingAmmo, ReserveAmmo);

    CurrentAmmo += ammoToLoad;
    ReserveAmmo -= ammoToLoad;

    IsReloading = false;
    NotifyAmmoChanged();
  }

  private void NotifyAmmoChanged()
  {
    OnAmmoChanged?.Invoke(CurrentAmmo, ReserveAmmo);
  }
}