using UnityEngine;

public class StartGameButton : MonoBehaviour, IRaycastHittable
{
  [SerializeField] private HideableObject menuToHide;
  [SerializeField] private float secondHideDelay = 5f;

  private bool sequenceStarted;

  public void OnRaycastHit()
  {
    Hit();
  }

  public void Hit()
  {
    if (sequenceStarted)
    {
      return;
    }

    if (menuToHide == null)
    {
      menuToHide = GetComponentInParent<HideableObject>();
    }

    if (menuToHide != null)
    {
      if (menuToHide.IsMoving)
      {
        return;
      }

      sequenceStarted = true;
      menuToHide.Hide();
      StartCoroutine(DelayedSecondHide());
      return;
    }

    HandleMenuHidden();
  }

  private System.Collections.IEnumerator DelayedSecondHide()
  {
    yield return new WaitForSeconds(secondHideDelay);

    if (menuToHide != null)
    {
      menuToHide.Hide(HandleMenuHidden);
      yield break;
    }

    HandleMenuHidden();
  }

  private void HandleMenuHidden()
  {
    if (GameManager.Instance != null)
    {
      GameManager.Instance.BeginGame();
    }
  }
}
