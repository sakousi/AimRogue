using UnityEngine;

public class StartGameButton : MonoBehaviour, IRaycastHittable
{
  [SerializeField] private HideableObject menuToHide;

  public void OnRaycastHit()
  {
    Hit();
  }

  public void Hit()
  {
    if (menuToHide == null)
    {
      menuToHide = GetComponentInParent<HideableObject>();
    }

    if (menuToHide != null)
    {
      menuToHide.Hide();
    }

    if (GameManager.Instance != null)
    {
      GameManager.Instance.BeginGame();
    }
  }
}