using UnityEngine;

public class PlayerCameraRecoil : MonoBehaviour
{
  [SerializeField] private float returnSpeed = 8f;
  [SerializeField] private float snappiness = 12f;

  private Vector3 currentRotation;
  private Vector3 targetRotation;

  private void Update()
  {
    targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
    currentRotation = Vector3.Lerp(currentRotation, targetRotation, snappiness * Time.deltaTime);

    transform.localRotation = Quaternion.Euler(currentRotation);
  }

  public void ApplyRecoil(float recoilX, float recoilY)
  {
    targetRotation += new Vector3(-recoilX, Random.Range(-recoilY, recoilY), 0f);
  }
}