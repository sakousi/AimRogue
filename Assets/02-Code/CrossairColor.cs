using UnityEngine;

public class CrossairColor : MonoBehaviour
{
    public Color color = Color.white;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //apply the color to the crossair as ui images
        foreach (Transform child in transform)
        {
            if (child.GetComponent<UnityEngine.UI.Image>() != null)
            {
                child.GetComponent<UnityEngine.UI.Image>().color = color;
            }
        }
    }
}
