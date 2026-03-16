using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRaycast : MonoBehaviour
{
    public float range = 100f;

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ShootRay();
        }
    }

    void ShootRay()
    {
        if (Camera.main == null)
        {
            return;
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, range))
        {
            startGame startGameTrigger = hit.collider.GetComponent<startGame>();
            if (startGameTrigger != null)
            {
                startGameTrigger.Hit();
                return;
            }

            ExitGame exitGame = hit.collider.GetComponent<ExitGame>();
            if (exitGame != null)
            {
                exitGame.Hit();
                return;
            }

            life legacyLife = hit.collider.GetComponent<life>();
            if (legacyLife != null)
            {
                if (ScoreManager.instance != null && !ScoreManager.instance.TryConsumeBullet())
                {
                    return;
                }

                legacyLife.Hit();
                return;
            }
        }

        if (ScoreManager.instance != null && ScoreManager.instance.TryConsumeBullet())
        {
            ScoreManager.instance.RegisterMiss();
        }
    }
}
