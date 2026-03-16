using UnityEngine;

public class PlayerCameraRecoil : MonoBehaviour
{
    [Header("Settings")]
    public float snappiness = 12f;
    public float returnSpeed = 8f;

    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private Quaternion initialLocalRotation;

    void Start()
    {
        initialLocalRotation = transform.localRotation;
    }

    public void ApplyRecoil(float recoilX, float recoilY)
    {
        targetRotation += new Vector3(
            recoilX,
            Random.Range(-recoilY, recoilY),
            0f
        );
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(
            targetRotation,
            Vector3.zero,
            returnSpeed * Time.deltaTime
        );

        currentRotation = Vector3.Lerp(
            currentRotation,
            targetRotation,
            snappiness * Time.deltaTime
        );

        transform.localRotation = initialLocalRotation * Quaternion.Euler(
            -currentRotation.x,
            currentRotation.y,
            0f
        );
    }
}