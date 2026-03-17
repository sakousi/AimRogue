using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Weapons/Weapon Config")]
public class WeaponConfig : ScriptableObject
{
    [Header("Fire")]
    public WeaponFireMode fireMode = WeaponFireMode.ShotByShot;
    public float fireRate = 10f;
    public int burstCount = 3;

    [Header("Ammo")]
    [Tooltip("-1 = chargeur infini")]
    public int magazineSize = 30;

    [Tooltip("-1 = réserve infinie")]
    public int startingReserveAmmo = 90;

    public float reloadDuration = 2f;

    [Header("Recoil")]
    public float weaponKickback = 0.1f;
    public float weaponRotationKick = 2f;
    public float cameraRecoilX = 2f;
    public float cameraRecoilY = 1f;
}