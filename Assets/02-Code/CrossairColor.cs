using UnityEngine;

public class CrossairColor : MonoBehaviour
{
    public Color color = Color.white;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().color = color;
    }
}
