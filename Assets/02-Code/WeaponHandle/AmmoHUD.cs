using TMPro;
using UnityEngine;

public class AmmoHUD : MonoBehaviour
{
    public WeaponReload weaponReload;
    public TextMeshProUGUI ammoText;

    void Start()
    {
        if (weaponReload != null)
        {
            weaponReload.OnAmmoChanged += UpdateAmmoDisplay;
            UpdateAmmoDisplay(weaponReload.currentAmmo, weaponReload.reserveAmmo);
        }
    }

    void OnDestroy()
    {
        if (weaponReload != null)
        {
            weaponReload.OnAmmoChanged -= UpdateAmmoDisplay;
        }
    }

    private void UpdateAmmoDisplay(int currentAmmo, int reserveAmmo)
    {
        if (ammoText == null)
            return;

        ammoText.text = currentAmmo + " / " + reserveAmmo;
    }
}