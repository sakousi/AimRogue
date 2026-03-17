using System;
using UnityEngine;

public class HideableObject : MonoBehaviour
{
  [SerializeField] private float previewOffsetY = -10f;
  [SerializeField] private float hiddenOffsetY = -10f;
  [SerializeField] private float moveSpeed = 5f;

  private Vector3 visiblePosition;
  private Vector3 previewPosition;
  private Vector3 hiddenPosition;
  private int currentStep;
  private int targetStep;
  private Action pendingHiddenCallback;

  private void Start()
  {
    visiblePosition = transform.position;
    previewPosition = visiblePosition + new Vector3(0f, previewOffsetY, 0f);
    hiddenPosition = visiblePosition + new Vector3(0f, hiddenOffsetY, 0f);
  }

  private void Update()
  {
    Vector3 target = GetPositionForStep(targetStep);

    transform.position = Vector3.MoveTowards(
        transform.position,
        target,
        moveSpeed * Time.deltaTime
    );

    if (transform.position != target)
    {
      return;
    }

    currentStep = targetStep;

    if (currentStep != 2 || pendingHiddenCallback == null)
    {
      return;
    }

    Action callback = pendingHiddenCallback;
    pendingHiddenCallback = null;
    callback.Invoke();
  }

  public void Hide(Action onHidden = null)
  {
    if (targetStep >= 2)
    {
      onHidden?.Invoke();
      return;
    }

    targetStep++;

    if (targetStep == 2 && onHidden != null)
    {
      pendingHiddenCallback += onHidden;
    }
  }

  public void Show()
  {
    currentStep = 0;
    targetStep = 0;
    pendingHiddenCallback = null;
    transform.position = visiblePosition;
  }

  public bool IsMoving => currentStep != targetStep;

  private Vector3 GetPositionForStep(int step)
  {
    return step switch
    {
      1 => previewPosition,
      2 => hiddenPosition,
      _ => visiblePosition,
    };
  }
}
