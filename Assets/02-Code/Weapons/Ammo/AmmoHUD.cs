using TMPro;
using UnityEngine;

public class AmmoHUD : MonoBehaviour
{
  [SerializeField] private WeaponReload weaponReload;
  [SerializeField] private TextMeshProUGUI ammoText;

  private void OnEnable()
  {
    if (weaponReload != null)
    {
      weaponReload.OnAmmoChanged += HandleAmmoChanged;
      HandleAmmoChanged(weaponReload.CurrentAmmo, weaponReload.ReserveAmmo);
    }
  }

  private void OnDisable()
  {
    if (weaponReload != null)
    {
      weaponReload.OnAmmoChanged -= HandleAmmoChanged;
    }
  }

  private void HandleAmmoChanged(int currentAmmo, int reserveAmmo)
  {
    if (ammoText != null)
    {
      ammoText.text = $"{currentAmmo} / {reserveAmmo}";
    }
  }
}