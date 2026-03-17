using System;
using System.Collections;
using UnityEngine;

public class WeaponReload : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip reloadSound;

    [Header("Ammo")]
    public int magazineSize = 30;
    public int currentAmmo;
    public int reserveAmmo = 90;

    [Header("Reload")]
    public float reloadTime = 2f;
    public bool isReloading = false;

    public event Action<int, int> OnAmmoChanged;

    void Start()
    {
        currentAmmo = magazineSize;
        NotifyAmmoChanged();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            if (currentAmmo < magazineSize && reserveAmmo > 0)
            {
                StartCoroutine(Reload());
            }
        }
    }

    public bool TryConsumeBullet()
    {
        if (isReloading)
            return false;

        if (currentAmmo <= 0)
            return false;

        currentAmmo--;
        NotifyAmmoChanged();
        return true;
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log("Reloading...");

        if (reloadSound != null)
        {
            AudioSource.PlayClipAtPoint(reloadSound, transform.position);
        }

        yield return new WaitForSeconds(reloadTime);

        int bulletsNeeded = magazineSize - currentAmmo;
        int bulletsToLoad = Mathf.Min(bulletsNeeded, reserveAmmo);

        currentAmmo += bulletsToLoad;
        reserveAmmo -= bulletsToLoad;

        NotifyAmmoChanged();

        Debug.Log("Reloaded!");
        isReloading = false;
    }

    private void NotifyAmmoChanged()
    {
        OnAmmoChanged?.Invoke(currentAmmo, reserveAmmo);
    }
}