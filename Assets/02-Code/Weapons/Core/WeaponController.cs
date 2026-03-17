using UnityEngine;

public class WeaponController : MonoBehaviour
{
  [Header("Config")]
  [SerializeField] private WeaponConfig weaponConfig;

  [Header("Components")]
  [SerializeField] private WeaponFire weaponFire;
  [SerializeField] private WeaponReload weaponReload;
  [SerializeField] private WeaponCameraRaycast weaponCameraRaycast;
  [SerializeField] private ShotFired shotFired;
  [SerializeField] private PlayerCameraRecoil playerCameraRecoil;

  public WeaponConfig Config => weaponConfig;
  public WeaponFire WeaponFire => weaponFire;
  public WeaponReload WeaponReload => weaponReload;
  public WeaponCameraRaycast WeaponCameraRaycast => weaponCameraRaycast;
  public ShotFired ShotFired => shotFired;
  public PlayerCameraRecoil PlayerCameraRecoil => playerCameraRecoil;
}