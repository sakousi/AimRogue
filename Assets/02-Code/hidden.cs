using UnityEngine;

public class hidden : MonoBehaviour
{
    public float hiddenOffsetY = -5f;
    public float moveSpeed = 5f;

    private Vector3 visiblePosition;
    private Vector3 hiddenPosition;
    private bool shouldHide;

    void Start()
    {
        visiblePosition = transform.position;
        
        hiddenPosition = visiblePosition + new Vector3(0f, hiddenOffsetY, 0f);
    }

    void Update()
    {
        if (!shouldHide)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            hiddenPosition,
            moveSpeed * Time.deltaTime);
    }

    public void Hide()
    {
        Debug.Log("[hidden] Hide called on " + gameObject.name);
        shouldHide = true;
    }

    public void Show()
    {
        Debug.Log("[hidden] Show called on " + gameObject.name);
        shouldHide = false;
        transform.position = visiblePosition;
    }
}
