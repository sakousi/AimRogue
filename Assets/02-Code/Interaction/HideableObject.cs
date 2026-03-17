using UnityEngine;

public class HideableObject : MonoBehaviour
{
  [SerializeField] private float hiddenOffsetY = -5f;
  [SerializeField] private float moveSpeed = 5f;

  private Vector3 visiblePosition;
  private Vector3 hiddenPosition;
  private bool shouldHide;

  private void Start()
  {
    visiblePosition = transform.position;
    hiddenPosition = visiblePosition + new Vector3(0f, hiddenOffsetY, 0f);
  }

  private void Update()
  {
    Vector3 target = shouldHide ? hiddenPosition : visiblePosition;

    transform.position = Vector3.MoveTowards(
        transform.position,
        target,
        moveSpeed * Time.deltaTime
    );
  }

  public void Hide()
  {
    shouldHide = true;
  }

  public void Show()
  {
    shouldHide = false;
  }
}